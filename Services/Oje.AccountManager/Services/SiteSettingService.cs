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
using Oje.Infrastructure.Enums;
using Oje.AccountService.Models.View;

namespace Oje.AccountService.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor httpContextAccessor = null;
        readonly IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService = null;
        readonly IPropertyService PropertyService = null;
        readonly IBlogService BlogService = null;
        readonly IOurObjectService OurObjectService = null;

        static List<SiteSetting> SS { get; set; }
        static Dictionary<string, string> mainServiceStr = new();

        public SiteSettingService(
                AccountDBContext db,
                IHttpContextAccessor httpContextAccessor,
                IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService,
                IPropertyService PropertyService,
                IBlogService BlogService,
                IOurObjectService OurObjectService
            )
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
            this.ExternalNotificationServiceConfigService = ExternalNotificationServiceConfigService;
            this.PropertyService = PropertyService;
            this.BlogService = BlogService;
            this.OurObjectService = OurObjectService;
        }

        public object GetightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title + "(" + t.WebsiteUrl + ")" }).ToList());

            return result;
        }

        public SiteSetting GetSiteSetting()
        {
            if (SS == null || SS.Count == 0)
            {
                if (SS == null || SS.Count == 0)
                    SS = db.SiteSettings
                        .Include(t => t.User).ThenInclude(t => t.Province)
                        .Include(t => t.User).ThenInclude(t => t.City)
                        .AsNoTracking()
                        .ToList();
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
            var curVer = GlobalConfig.GetAppVersion();
            var siteSetting = GetSiteSetting();
            string curKey = curVer + "_" + siteSetting?.Id;

            if (mainServiceStr == null)
                mainServiceStr = new();

            if(mainServiceStr != null && mainServiceStr.Keys.Any(t => t == curKey))
            {
                var tempResult = mainServiceStr[curKey];
                if (!string.IsNullOrEmpty(tempResult))
                    return tempResult;
            }

            string result = "const mainServiceName = 'mainServiceName';" + Environment.NewLine;

            
            var topLeftIconList = PropertyService.GetBy<MainPageTopLeftIconVM>(PropertyType.MainPageTopLeftIcon, siteSetting?.Id);
            var aboutUsMainPage = PropertyService.GetBy<AboutUsMainPageVM>(PropertyType.AboutUsMainPage, siteSetting?.Id);
            var ourPrideMainPage = PropertyService.GetBy<OurPrideVM>(PropertyType.OurPrideMainPage, siteSetting?.Id);
            var remindUsMainPage = PropertyService.GetBy<ReminderMainPageVM>(PropertyType.RemindUsMainPage, siteSetting?.Id);
            var footerDescrption = PropertyService.GetBy<FooterDescrptionVM>(PropertyType.FooterDescrption, siteSetting?.Id);
            var allMainBlog = BlogService.GetMainBlog(4, siteSetting?.Id);
            var allOurObject = OurObjectService.GetList(siteSetting?.Id);


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
            result += "'/Modules/Assets/MainPage/linkedin.svg',";
            result += "'/Home/GetReminderConfig',";
            result += "'/Home/GetOurPrideMainPage',";
            result += "'/Home/GetAboutUsMainPage',";
            result += "'/Blog/Blog/GetMainBlog',";
            result += "'/Question/YourQuestion/GetList',";
            result += "'/Home/GetOtherInsuranceConfig',";
            result += "'/ProposalFormInquiries/CarThirdPartyInquiry/GetJsonConfig',";
            result += "'/ProposalFormInquiries/CarBodyInquiry/GetJsonConfig',";
            result += "'/ProposalFormInquiries/FireInsurance/GetJsonConfig',";
            result += "'/Reminder/GetMainPageDescription',";
            result += "'/Question/ProposalFormYourQuestion/GetInquiryList?fid=3',";
            result += "'/Question/ProposalFormYourQuestion/GetInquiryList?fid=2',";
            result += "'/Question/ProposalFormYourQuestion/GetInquiryList?fid=1',";
            result += "'/Home/GetTopLeftIconList',";
            result += "'/home/GetFooterSocialUrl',";
            result += "'/Home/GetLoginModalConfig',";
            result += "'/TopMenu/GetTopMenu',";
            result += "'/Home/GetFooterDescrption',";
            result += "'/Home/GetFooterExteraLink',";
            result += "'/Home/GetFooterExteraLinkGroup',";
            result += "'/Home/GetFooterSambole',";
            result += "'/Home/GetFooterInfor',";
            if (topLeftIconList != null)
            {
                if (!string.IsNullOrEmpty(topLeftIconList.mainFile1_address))
                    result += "'" + topLeftIconList.mainFile1_address + "',";
                if (!string.IsNullOrEmpty(topLeftIconList.mainFile2_address))
                    result += "'" + topLeftIconList.mainFile2_address + "',";
                if (!string.IsNullOrEmpty(topLeftIconList.mainFile3_address))
                    result += "'" + topLeftIconList.mainFile3_address + "',";
            }

            if (aboutUsMainPage != null)
            {
                if (!string.IsNullOrEmpty(aboutUsMainPage.rightFile_address))
                    result += "'" + aboutUsMainPage.rightFile_address + "',";
                if (!string.IsNullOrEmpty(aboutUsMainPage.centerFile_address))
                    result += "'" + aboutUsMainPage.centerFile_address + "',";
                if (!string.IsNullOrEmpty(aboutUsMainPage.leftFile_address))
                    result += "'" + aboutUsMainPage.leftFile_address + "',";
            }

            if (ourPrideMainPage != null)
            {
                if (!string.IsNullOrEmpty(ourPrideMainPage.image1_address))
                    result += "'" + ourPrideMainPage.image1_address + "',";
                if (!string.IsNullOrEmpty(ourPrideMainPage.image2_address))
                    result += "'" + ourPrideMainPage.image2_address + "',";
                if (!string.IsNullOrEmpty(ourPrideMainPage.image3_address))
                    result += "'" + ourPrideMainPage.image3_address + "',";
                if (!string.IsNullOrEmpty(ourPrideMainPage.image4_address))
                    result += "'" + ourPrideMainPage.image4_address + "',";
            }

            if (remindUsMainPage != null)
                if (!string.IsNullOrEmpty(remindUsMainPage.mainImage_address))
                    result += "'" + remindUsMainPage.mainImage_address + "',";

            if (footerDescrption != null)
            {
                if (!string.IsNullOrEmpty(footerDescrption.logo1_address))
                    result += "'" + footerDescrption.logo1_address + "',";
                if (!string.IsNullOrEmpty(footerDescrption.logo2_address))
                    result += "'" + footerDescrption.logo2_address + "',";
                if (!string.IsNullOrEmpty(footerDescrption.logo3_address))
                    result += "'" + footerDescrption.logo3_address + "',";
            }

            if (allMainBlog != null)
            {
                foreach(var blog in allMainBlog)
                {
                    if (!string.IsNullOrEmpty(blog.ImageUrl200))
                        result += "'" + GlobalConfig.FileAccessHandlerUrl + blog.ImageUrl200 + "',";
                    if (!string.IsNullOrEmpty(blog.ImageUrl600))
                        result += "'" + GlobalConfig.FileAccessHandlerUrl + blog.ImageUrl600 + "',";
                }
            }

            if (allOurObject != null)
                foreach (var ourO in allOurObject)
                    if (!string.IsNullOrEmpty(ourO.ImageUrl))
                        result += "'" + GlobalConfig.FileAccessHandlerUrl + ourO.ImageUrl + "',";

            if (siteSetting != null)
            {
                if (!string.IsNullOrEmpty(siteSetting.Image96))
                    result += "'" + GlobalConfig.FileAccessHandlerUrl + siteSetting.Image96 + "',";
                if (!string.IsNullOrEmpty(siteSetting.Image192))
                    result += "'" + GlobalConfig.FileAccessHandlerUrl + siteSetting.Image192 + "',";
                if (!string.IsNullOrEmpty(siteSetting.Image512))
                    result += "'" + GlobalConfig.FileAccessHandlerUrl + siteSetting.Image512 + "',";
                if (!string.IsNullOrEmpty(siteSetting.ImageText))
                    result += "'" + GlobalConfig.FileAccessHandlerUrl + siteSetting.ImageText + "',";
            }

            result += "'/Home/GetOurCompanyList',";
            result += "'/Home/GetOurCustomerList'";
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

            result += @"
                self.addEventListener('push', function(e) {
                if (!(self.Notification && self.Notification.permission === 'granted')) {
                    return;
                }
                  var data = e.data.json();

                    var options = {
                        body: data.body,
                        icon: '/Modules/Assets/support.png',
                        vibrate: [100, 50, 100],
                        data: {
			                url: data.url 
                        },
                        actions: [
                            {
                                action: 'explore', title: 'مشاهده',
                                icon: '/Modules/Assets/view.png'
                            },
                            {
                                action: 'close', title: 'بستن',
                                icon: '/Modules/Assets/close.png'
                            },
                        ]
                    };
                    e.waitUntil(
                        self.registration.showNotification(data.title, options)
                    );
                });
            ";

            mainServiceStr[curKey] = result;

            return result;
        }

        public string GetRegisterServices()
        {
            int? siteSettingId = GetSiteSetting()?.Id;
            var getActiveNotificationConfig = ExternalNotificationServiceConfigService.GetActiveConfig(siteSettingId);

            string result = @"
            function subscribeUser() {
            
            if ('serviceWorker' in navigator) {
                navigator.serviceWorker.ready.then(function (reg) {
            
            setTimeout(function() {
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
                            console.log('Permission for notifications was denied');
                        } else {
                            console.log('Unable to subscribe to push', e);
                        }
                    });

            }, 8000);

                   
                });
                window.addEventListener('load', function () {
                    navigator.serviceWorker
                        .register('/serviceWorker.js?v=" + GlobalConfig.GetAppVersion() + @"')
                        .then(reg => {

                            setTimeout(function() {
                                reg.pushManager.getSubscription().then(function (sub) {
                                    if (sub === null) {
                                        // Update UI to ask user to register for Push
                                        console.log('Not subscribed to push service!');
                                    } else {
                                        console.log('subscribed to push service!');
                                        //sub.unsubscribe();
                                        // We have a subscription, update the database
                                    }
                                });
                                
                            }, 10000);
                            
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
