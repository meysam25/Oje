using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.AccountService.Services.EContext;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;

namespace Oje.AccountService.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor httpContextAccessor = null;
        readonly IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService = null;


        static List<SiteSetting> SS { get; set; }
        static object lockObject = new object();

        public SiteSettingService(
                AccountDBContext db,
                IHttpContextAccessor httpContextAccessor,
                IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService
            )
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
            this.ExternalNotificationServiceConfigService = ExternalNotificationServiceConfigService;
        }

        public object GetightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title + "(" + t.WebsiteUrl + ")" }).ToList());

            return result;
        }

        public SiteSetting GetSiteSetting()
        {
            if (lockObject == null)
                lockObject = new object();
            if (SS == null || SS.Count == 0)
            {
                lock (lockObject)
                {
                    if (SS == null || SS.Count == 0)
                        SS = db.SiteSettings.Include(t => t.User).AsNoTracking().ToList();
                }
            }
            string host = httpContextAccessor.HttpContext?.Request?.Host.Host;
            if (!string.IsNullOrEmpty(host))
                return SS.Where(t => t.WebsiteUrl == host || t.PanelUrl == host).FirstOrDefault();
            return null;
        }

        public void UpdateSiteSettings()
        {
            SS = db.SiteSettings.Include(t => t.User).AsNoTracking().ToList();
        }

        public object GetManifest()
        {
            var foundSetting = GetSiteSetting();
            if (foundSetting == null)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            var getActiveNotificationConfig = ExternalNotificationServiceConfigService.GetActiveConfig(foundSetting?.Id);

            return new
            {
                name = foundSetting.Title,
                short_name = foundSetting.Title,
                start_url = "/",
                display = "fullscreen",
                background_color = "#fdfdfd",
                theme_color = "#db4938",
                orientation = "portrait-primary",
                scope = "/",
                gcm_sender_id = getActiveNotificationConfig?.PublicKey,
                description = foundSetting.SeoMainPage,
                icons = new List<object>()
                {
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image96,
                        type = "image/png",
                        sizes = "96x96"
                    },
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image192,
                        type = "image/png",
                        sizes = "192x192"
                    },
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image512,
                        type = "image/png",
                        sizes = "512x512",
                        purpose = "any maskable"
                    }
                }
            };
        }

        public string GetMainService()
        {
            string result = "const mainServiceName = 'mainServiceName';" + Environment.NewLine;

            var curVer = GlobalConfig.GetAppVersion();

            result += "const assets = [";
            result += "'/',";
            result += "'/Modules/Core/css/master.min.css.gz?v=" + curVer + "',";
            result += "'/Modules/Core/css/websiteCore.min.css.gz?v=" + curVer + "',";
            result += "'/Modules/Core/css/mainPage.min.css.gz?v=" + curVer + "',";
            result += "'/Modules/Core/js/jquery.min.js.gz?v=" + curVer + "',";
            result += "'/Modules/Core/js/websiteCor.min.js.gz?v=" + curVer + "',";
            result += "'/Modules/Core/js/mainPage.min.js.gz?v=" + curVer + "',";
            result += "'/Modules/Core/js/registerServices.min.js.gz?v=" + curVer + "',";
            result += "'/Modules/Core/js/chart.min.js.gz?v=" + curVer + "',";
            result += "'/Modules/Webfonts/Vazir/Vazir-Regular-FD.woff2',";
            result += "'/Modules/webfonts/fa-light-300.woff2',";
            result += "'/Modules/Assets/MainPage/electronDevelopment.png',";
            result += "'/Modules/Assets/MainPage/bgp.png',";
            result += "'/Modules/Assets/MainPage/salecIcon.png',";
            result += "'/Modules/Assets/MainPage/badane.png',";
            result += "'/Modules/Assets/MainPage/atashsozi.png',";
            result += "'/Modules/Images/iran.png',";
            result += "'/Modules/Assets/MainPage/etehadiyeLogo.png',";
            result += "'/Modules/Assets/MainPage/telegram.svg',";
            result += "'/Modules/Assets/MainPage/twitter.svg',";
            result += "'/Modules/Assets/MainPage/instagram.svg',";
            result += "'/Modules/Assets/MainPage/linkedin.svg'";
            result += "];";

            result += @"
                self.addEventListener('install', installEvent => {
                  installEvent.waitUntil(
                    caches.open(mainServiceName).then(cache => {
                      cache.addAll(assets);
                    })
                  );
                });
            ";

            result += @"
                self.addEventListener('fetch', fetchEvent => {
                  if (fetchEvent.request.method != 'GET') return;
                  fetchEvent.respondWith(
                    caches.match(fetchEvent.request).then(res => {
                      return res || fetch(fetchEvent.request);
                    })
                  );
                });
            ";

            result += @"
                self.addEventListener('notificationclick', function(e) {
                    let url = e.notification.data.url;
                    var notification = e.notification;
                    var action = e.action;
                    if (action === 'close') {
                        notification.close();
                    } else {
                        e.waitUntil(
                            clients.matchAll({type: 'window'}).then( windowClients => {
                                for (var i = 0; i < windowClients.length; i++) {
                                    var client = windowClients[i];
                                    if (client.url === url && 'focus' in client) {
                                        return client.focus();
                                    }
                                }
                                if (clients.openWindow) {
                                    return clients.openWindow(url);
                                }
                            })
                        );
                        notification.close();
                    }
                });
            ";

            //result += "setInterval(function() {self.registration.showNotification('PWA Notification!', { body: 'Testing Our Notification', icon: './bell.png',data: {url: 'https://localhost:5001/Contract/Index'}});}, 20000);";

            result += @"
                self.addEventListener('push', function(e) {
                if (!(self.Notification && self.Notification.permission === 'granted')) {
                    return;
                }
                  var data = e.data.json();

                    var options = {
                        body: data.body,
                        icon: 'images/icon-512x512.png',
                        vibrate: [100, 50, 100],
                        data: {
			                url: data.url 
                        },
                        actions: [
                            {
                                action: 'explore', title: 'Go interact with this!',
                                icon: 'images/checkmark.png'
                            },
                            {
                                action: 'close', title: 'Ignore',
                                icon: 'images/red_x.png'
                            },
                        ]
                    };
                    e.waitUntil(
                        self.registration.showNotification(data.title, options)
                    );
                });
            ";

            //result += "self.showNotification('PWA Notification!', { body: 'Testing Our Notification', icon: './bell.png',data: {url: 'https://google.com'}});";


            return result;
        }

        public string GetRegisterServices()
        {
            int? siteSettingId = GetSiteSetting()?.Id;
            var getActiveNotificationConfig = ExternalNotificationServiceConfigService.GetActiveConfig(siteSettingId);

            string result = @"
            function subscribeUser() {
            Notification.requestPermission(status => {
                if (status === 'granted') {
                } else {
                }
            });
            if ('serviceWorker' in navigator) {
                navigator.serviceWorker.ready.then(function (reg) {
            
                    reg.pushManager.subscribe({
                        userVisibleOnly: true
                        " + (getActiveNotificationConfig != null ? ",applicationServerKey: '" + getActiveNotificationConfig.PublicKey + "'" : "") + @"
                    }).then(function (sub) {
                        var postFormData = new FormData();
                        postFormData.append('auth', arrayBufferToBase64(sub.getKey('auth')));
                        postFormData.append('p256DH', arrayBufferToBase64(sub.getKey('p256dh')));
                        postFormData.append('endpoint', sub.endpoint);
                        postForm('/Home/PushNotificationSubscribe', postFormData);
                    }).catch(function (e) {
                        if (Notification.permission === 'denied') {
                            console.warn('Permission for notifications was denied');
                        } else {
                            console.error('Unable to subscribe to push', e);
                        }
                    });
                });
                window.addEventListener('load', function () {
                    navigator.serviceWorker
                        .register('/serviceWorker.js?v=" + GlobalConfig.GetAppVersion() + @"')
                        .then(reg => {
                            reg.pushManager.getSubscription().then(function (sub) {
                                if (sub === null) {
                                    // Update UI to ask user to register for Push
                                    console.log('Not subscribed to push service!');
                                } else {
                                    
                                    //sub.unsubscribe();
                                    // We have a subscription, update the database
                                }
                            });
                        })
                        .catch(err => console.log('service worker not registered', err));
                });
            }
        }
        function arrayBufferToBase64(buffer) {
            var binary = '';
            var bytes = new Uint8Array(buffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            return window.btoa(binary);
        }
        subscribeUser();
            
";



            return result;
        }
    }
}
