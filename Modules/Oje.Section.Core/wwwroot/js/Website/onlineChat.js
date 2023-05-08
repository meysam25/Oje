

function initOnlineChat(id, isAdmin) {
    var template = `
<span class="onlineChat" >
    <span class="onlineChatShadowBox" ></span>
    <span class="onlineChatHeader">
        <span class="onlineChatHeaderMenueButton" ><span></span><span></span><span></span></span>
        <span class="onlineChatHeaderTitleSection" ></span>
        <span class="onlineChatHeaderActionButtons" >
            <span class="fa fa-times onlineChatHeaderActionButtonClose" ></span>
        </span>
    </span>
    <span class="onlineChatMessages" >
    </span>
    <span class="onlineChatInputs">
        <span class="onlineChatInputInner">
            <textarea placeholder="پیغام شما" maxlength="4000" class="ocUserInputTextBox"></textarea>
            <span class="messageButton fa fa-paperclip" ></span>
            <span class="messageButton fa fa-camera" ></span>
        </span>
        <span style="margin-right:5px;" class="ocSendButton fa fa-microphone " ></span>

    </span>
    <span class="onlineChatMenue mainMenue" >
        <span class="onlineChatMenueItem enableDisableNotifiSound" ><span class="fa fa-volume-mute"></span>فعال کردن صدا</span>
        <span class="onlineChatMenueItem rigisterEmail" ><span class="fa fa-at"></span> ثبت ایمیل</span>
        <span class="onlineChatMenueItem sendMap" ><span class="fa fa-map-marker-alt"></span>ارسال نقشه</span>
    </span>
    <span class="onlineChatMenue emailMenue" >
        <span class="onlineChatMenueHeader" ><span class="fa fa-angle-right moveBackButtonForRigisterMobile"  ></span>ثبت ایمیل</span>
        <input class="menuInput" type="text" placeholder="ایمیل" />
        <a class="button mainBtn" >ذخیره</a>
        <a class="button secountBtn cancellAllMenus">انصراف</a>
    </span>
    <span class="onlineChatMenue uploadFile" >
        <span class="onlineChatMenueHeader" ><span class="fa fa-angle-right moveBackButtonForRigisterMobile"></span>ارسال فایل</span>
        <span class="fileUploadCl button mainBtn">
            ارسال
            <input accept=".jpg,.jpeg,.png,.doc,.pdf" type="file" name="mainFile" placeholder="لطفا فایل را انتخاب کنید" />
        </span>
        <a class="button secountBtn cancellAllMenus">انصراف</a>
    </span>
    <span class="onlineChatMenue sendMapMenu" >
        <span class="onlineChatMenueHeader" ><span class="fa fa-angle-right moveBackButtonForRigisterMobile"></span>ارسال نقشه</span>
        <span class="inputMapHolder" >در حال بارگزاری نقشه</span>
        <a class="button mainBtn" >ارسال</a>
        <a class="button secountBtn cancellAllMenus">انصراف</a>
    </span>
</span>

`;

    if ($('.onlineChat').length > 0) {
        $('.onlineChat')[0].close();
    }
    $('body').append(template);
    initOnlineChantJS(isAdmin, id);
}

function initOnlineChantJS(isAdmin, id) {
    if ($('.onlineChat').length > 0) {
        var onlineChatObj = $('.onlineChat')[0];

        onlineChatObj.connectionTId = id;
        initOnlineChantGetOrCreateNewId(onlineChatObj);
        initOnlineChantInitMessages(onlineChatObj);
        initOnlineChantNotification(onlineChatObj);
        initOnlineChantHeaderButtons(onlineChatObj);
        initOnlineChantActiveAndDeactiveNotifySound(onlineChatObj);
        initOnlineChantRigesterEmail(onlineChatObj);
        initOnlineChantUploadNewFile(onlineChatObj, isAdmin);
        initOnlineChantShowMapMenu(onlineChatObj, isAdmin);
        initOnlineChantInitCancellButton(onlineChatObj);
        initOnlineChantUserTyping(onlineChatObj);
        initOnlineChantSendMessage(onlineChatObj, isAdmin);
        initOnlineChantSocket(onlineChatObj, isAdmin);
        initOnlineChantInitAskForLoginMethods(onlineChatObj);
        initOnlineChantInitWhatToDoAfterUserLogin(onlineChatObj);

        bindCompanyTitle(onlineChatObj);
    }
}

function bindCompanyTitle(onlineChatObj) {
    postForm('/Home/GetCompanyTitle', new FormData(), function (res)
    {
        if (res && res.constructor == String) {
            $(this.curObj).find('.onlineChatHeaderTitleSection').text(res);
        }
    }.bind({ curObj: onlineChatObj })
    );
}

function initOnlineChantInitWhatToDoAfterUserLogin(onlineChatObj) {
    if (window['whatToDoAfterUserLogin']) {
        whatToDoAfterUserLogin.push(
            {
                key: 'onlineChat',
                curFun: function () {
                    this.onlineChatObj.close();
                    setTimeout(function () { initOnlineChat(); }, 100);
                }.bind({ onlineChatObj: onlineChatObj })
            });
    }
}

function initOnlineChantInitAskForLoginMethods(onlineChatObj) {
    onlineChatObj.askUserForLogin = function () {
        this.addNewMessage({
            message: 'جهت انجام این کار باید ابتدا لاگین کنید',
            isAdmin: true,
            cTime: new Date().getHours() + ':' + new Date().getMinutes(),
            type: 'loginQuestion'
        });
    };
    onlineChatObj.pintUserLoginQuestion = function (msgObj) {
        if (msgObj && msgObj.message) {
            $(this).find('.onlineChatMessages').append(this.getLoginQuestionTemplate(msgObj));
            this.moveScrollToBottom();
            this.bindYesOrNoLoginButtonEvent();
        }
    }
    onlineChatObj.getLoginQuestionTemplate = function (msgObj) {
        return `
          <span class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')}">
            <span class=" userIcon" ><span class="fa fa-user" ></span></span>
            <span class="userMessage"><span>${msgObj.message + (this.getYesOrNoForLogin())}</span></span>
            <span class="userTime" >${msgObj.cTime}</span>
        </span>
`;
    }
    onlineChatObj.getYesOrNoForLogin = function () {
        return `
        <span class="yesOrNoCtrl" >
            <span class="holderYesOrNoButton">
                <span class="yesOrNoeMessageButton yesButton fa fa-check" ></span>
                <span class="yesOrNoeMessageButton noButton fa fa-do-not-enter" ></span>
            </span>
            <span class="yesOrNoCtrlTitle" >آیا تمایل به ورود دارید ؟</span>
        </span>
`
    }
    onlineChatObj.bindYesOrNoLoginButtonEvent = function () {
        var sQuiry = $(this).find('.onlineChatMessages');
        var lastItem = sQuiry.find('.onlineChatMessageItem:last-child');
        if (lastItem.find('.yesOrNoeMessageButton').length > 0) {
            lastItem.find('.yesOrNoeMessageButton').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var sQuiry = $(this).closest('.holderYesOrNoButton');
                if (!sQuiry.hasClass('questionButtonSelectedSelected')) {
                    var isLike = $(this).hasClass('yesButton');
                    $(this).addClass('showJustThisBUtton');
                    sQuiry.addClass('questionButtonSelectedSelected');
                    $(this).closest('.yesOrNoCtrl').css('right', '0px');
                    if (isLike) {
                        initOnlineChantUserLogin($(this).closest('.onlineChat')[0]);
                    }
                }
            });
        }
    }
}

function initOnlineChantGetOrCreateNewId(onlineChatObj) {
    var newId = localStorage.getItem('onlineChatSupportId');
    if (!newId) {
        newId = uuidv4RemoveDash();
        localStorage.setItem('onlineChatSupportId', newId);
    }
    onlineChatObj.clientId = newId;
}

function initOnlineChantSocket(onlineChatObj, isAdmin) {
    if (window['signalR']) {
        var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Information).withUrl("/support").build();
        connection.on("groupMessages", function (groupMessageItems) {
            this.onlineChatObj.addGroupMessages(groupMessageItems);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("joinSuccess", function () {
            if (this.isAdmin)
                this.onlineChatObj.loadMessageForAdmin(this.onlineChatObj.connectionTId);
            else
                this.onlineChatObj.loadMessage(this.onlineChatObj.clientId);
        }.bind({ onlineChatObj: onlineChatObj, isAdmin: isAdmin }));
        connection.on("reciveUserMessageForAdmin", function (message) {
            message.isAdmin = true;
            this.onlineChatObj.addNewMessage(message);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("notification", function (nObj) {
            if (!nObj.isError)
                this.onlineChatObj.closeAllMenus();
            this.onlineChatObj.newNotification(nObj);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("reciveUserMessage", function (message) {
            message.isAdmin = true;
            this.onlineChatObj.addNewMessage(message);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("typing", function () {
            this.onlineChatObj.showUserTyping();
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("userIdForUploadingFile", function (userId) {
            this.onlineChatObj.uploadFileForAdmin(userId);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("userIdForUploadingVoice", function (userId) {
            this.onlineChatObj.uploadVoiceForAdminUrlFirst(userId);
        }.bind({ onlineChatObj: onlineChatObj }));
        connection.on("messageHistory", function (messages) {
            if (messages && messages.length > 0) {
                for (var i = 0; i < messages.length; i++) {
                    this.onlineChatObj.addNewMessage(messages[i]);
                }
            }
        }.bind({ onlineChatObj: onlineChatObj }));

        var wacherInterval = null;
        connection.onclose(function (e) {
            if (this.connection.dontTryAgin)
                return;
            wacherInterval = setInterval(function () {
                if (connection.state == 'Disconnected') {
                    connection.start();
                }
                else if (connection.state == 'Connected') {
                    clearInterval(wacherInterval);
                }
            }, 3000);
        }.bind({ connection }))
        connection.start().then(function () {
            this.curThis.newNotification({ isError: false, message: 'ارتباط برقرار شد' });
            this.curThis.join(this.curThis.connectionTId);

        }.bind({ curThis: onlineChatObj, isAdmin: isAdmin })).catch(function (err) {
            this.curThis.newNotification({ isError: true, message: err.toString() });
            return console.log(err.toString());
        }.bind({ curThis: onlineChatObj }));
        onlineChatObj.connection = connection;

        onlineChatObj.join = function (toClientId) {
            this.connection.invoke("Join", this.clientId, toClientId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.userTyping = function () {
            this.connection.invoke("UserTyping", this.clientId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.userTypingForAdmin = function () {
            this.connection.invoke("UserTypingForAdmin", this.connectionTId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.uploadFileUser = function (link) {
            this.connection.invoke("UploadFileUser", this.clientId, link).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.getUserIdAndUploadFile = function (uploadFormData) {
            this.pendingUploadFile = uploadFormData;
            this.connection.invoke("GetUserId", this.connectionTId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.getUserIdAndUploadVoice = function () {
            this.connection.invoke("GetUserIdForVoice", this.connectionTId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.uploadFileAdmin = function (link) {
            this.connection.invoke("UploadFileAdmin", this.connectionTId, link).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.selectGroup = function (groupId) {
            this.connection.invoke("SelectGroup", groupId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.likeAnswer = function (groupId, isLike) {
            this.connection.invoke("LikeDislike", groupId, isLike).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.uploadVoiceForUser = function (link) {
            this.connection.invoke("UploadVoiceForUser", this.clientId, link).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.uploadVoiceForAdmin = function (link) {
            this.connection.invoke("UploadVoiceForAdmin", this.connectionTId, link).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.sendMessage = function (message) {
            this.connection.invoke("SendMessage", message, this.clientId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.sendMapForUser = function (mapObj) {
            this.connection.invoke("SendMapForUser", mapObj, this.clientId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.sendMapForAdmin = function (mapObj) {
            this.connection.invoke("SendMapForAdmin", mapObj, this.connectionTId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.sendMessageForAdmin = function (message, targetConnectionId) {
            this.connection.invoke("SendMessageForAdmin", message, targetConnectionId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        }
        onlineChatObj.loadMessage = function () {
            this.connection.invoke('GetMessageList', this.clientId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        };
        onlineChatObj.loadMessageForAdmin = function (targetConnectionId) {
            this.connection.invoke('GetMessageListForAdmin', targetConnectionId).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        };
        onlineChatObj.subScribeEmail = function (email) {
            this.connection.invoke('SubscribeEmail', email).catch(function (err) {
                this.curThis.newNotification({ isError: true, message: err.toString() });
                return console.error(err.toString());
            }.bind({ curThis: this }));
        };
    }
}

function initOnlineChantUserLogin(onlineChatObj) {
    if (!isUserLogin) {
        postForm('/Home/GetLoginModalConfig', new FormData(), function (res) {
            if (res && res.panels && res.panels[0].moduals && res.panels[0].moduals[0].panels) {
                $('#' + res.panels[0].id).remove();
                if ($('#' + res.panels[0].moduals[0].panels[0].id).length > 0)
                    $('#' + res.panels[0].moduals[0].panels[0].id).remove();

                generateForm(res.panels[0].moduals[0], $(onlineChatObj).find('.onlineChatMessages'), true);
                onlineChatObj.moveScrollToBottom();
            }
        }, null, null, null, 'GET');

    }
}

function initOnlineChantSendMessage(onlineChatObj, isAdmin) {
    onlineChatObj.isMic = function () {
        return $(this).find('.ocSendButton').hasClass('fa-microphone')
    };
    $(onlineChatObj).find('.ocUserInputTextBox').keyup(function () {
        var curValue = $(this).val();
        if (curValue) {
            $(this).closest('.onlineChatInputs').find('.ocSendButton').removeClass('fa-microphone').addClass('fa-arrow-up');
        } else {
            $(this).closest('.onlineChatInputs').find('.ocSendButton').removeClass('fa-arrow-up').addClass('fa-microphone');
        }
    });

    $(onlineChatObj).find('.ocUserInputTextBox').keypress(function () {
        if (isAdmin)
            this.onlineChatObj.userTypingForAdmin();
        else
            this.onlineChatObj.userTyping();

    }.bind({ onlineChatObj: onlineChatObj }));

    $(onlineChatObj).find('.ocUserInputTextBox').on('input', function () {
        if (isAdmin)
            this.onlineChatObj.userTypingForAdmin();
        else
            this.onlineChatObj.userTyping();

    }.bind({ onlineChatObj: onlineChatObj }));

    $(onlineChatObj).find('.ocSendButton').mousedown(function () {
        if (onlineChatObj.isMic()) {
            onlineChatObj.recordAudio();
            $(this).css('background-color', 'red');
        }
    });
    $(onlineChatObj).find('.ocSendButton').bind('touchstart', function () {
        if (onlineChatObj.isMic()) {
            onlineChatObj.recordAudio();
            $(this).css('background-color', 'red');
        }
    });

    $(onlineChatObj).find('.ocSendButton').mouseup(function () {
        if (onlineChatObj.isMic()) {
            $(this).css('background-color', '#0e72ed');
            onlineChatObj.stopAndPostRecordAudio();
        }
    });
    $(onlineChatObj).find('.ocSendButton').bind('touchend', function (e) {
        if (onlineChatObj.isMic()) {
            $(this).css('background-color', '#0e72ed');
            onlineChatObj.stopAndPostRecordAudio();
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    });

    $(onlineChatObj).find('.ocSendButton').click(function () {
        var onlineChatObj = $(this).closest('.onlineChat')[0];
        if (onlineChatObj.isMic()) {
        } else {
            var quirySelector = $(this).closest('.onlineChatInputs').find('.ocUserInputTextBox');
            var message = quirySelector.val();
            if (isAdmin == true)
                onlineChatObj.sendMessageForAdmin(message, onlineChatObj.connectionTId);
            else
                onlineChatObj.sendMessage(message);

            onlineChatObj.addTextMessage({ message: message, cTime: new Date().getHours() + ':' + new Date().getMinutes() });
            quirySelector.val('');
            quirySelector.keyup();
        }
    });
    initAutoHeightForInputText(onlineChatObj);
    onlineChatObj.updateInputHeight = function (newHeight) {
        newHeight += 4;
        if (newHeight >= 30) {
            $(onlineChatObj).find('.ocUserInputTextBox').css('height', newHeight + 'px');
            $(onlineChatObj).find('.onlineChatMessages').css('height', 'calc(100% - ' + (56 + 30 + newHeight) + 'px)');
        }
    }

    initRecordAndPostAudioFunction(onlineChatObj, isAdmin);

    onlineChatObj.addVoice = function (msgObj)
    {
        $(this).find('.onlineChatMessages').append(`
         <span class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')}">
            <span class=" userIcon" ><span class="fa fa-user" ></span></span>
            <span class="userMessage"><span style="display:block;width:100%;" ><audio style="width:100%;" controls><source src="${msgObj.link}" type="audio/ogg"></audio></span>
        </span>
`);
    };
}

function detectBrowser() {
    if ((navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1) {
        return 'Opera';
    } else if (navigator.userAgent.indexOf("Chrome") != -1) {
        return 'Chrome';
    } else if (navigator.userAgent.indexOf("Safari") != -1) {
        return 'Safari';
    } else if (navigator.userAgent.indexOf("Firefox") != -1) {
        return 'Firefox';
    } else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) {
        return 'IE';//crap
    } else {
        return 'Unknown';
    }
}

function initRecordAndPostAudioFunction(onlineChatObj, isAdmin) {
    onlineChatObj.chunks = [];
    onlineChatObj.recordAudio = function () {
        if (!isUserLogin) {
            this.newNotification({ isError: true, message: 'لطفا ابتدا لاگین کنید' });
            this.askUserForLogin();
        }
        if (!navigator.mediaDevices.getUserMedia)
            this.newNotification({ isError: true, message: 'امکان ارسال صدا در مرورگر شما وجود ندارد' });

        navigator.mediaDevices.getUserMedia({ audio: true, video: false }).then(function (stream) {
            var mrOption = { 'type': 'audio/webm; codecs=opus', 'mimeType': 'audio/webm; codecs=opus' };
            var isFireFox = detectBrowser() == 'Firefox';
            if (isFireFox == true)
                mrOption = { 'type': 'audio/ogg; codecs=opus', 'mimeType': 'audio/ogg; codecs=opus' };
            const mediaRecorder = new MediaRecorder(stream, mrOption);
            this.curThis.mediaRecorder = mediaRecorder;
            mediaRecorder.onstop = function () {
                var curDate = new Date();
                var clipName = curDate.getHours() + '-' + curDate.getMinutes() + '-' + curDate.getSeconds() + '-' + curDate.getMilliseconds();
                const blob = new Blob(this.curThis.chunks, mrOption);
                var f2 = new File([blob], clipName + (isFireFox? '.ogg' : '.webm'), mrOption);
                this.curThis.chunks = [];
                var postFormData = new FormData();
                postFormData.append('mainFile', f2);
                if (!isAdmin) {
                    postForm('/Home/UploadNewVoiceForOnlineChat', postFormData, function (res) {

                        if (res && (typeof res === 'string' || res instanceof String)) {

                            onlineChatObj.addNewMessage({ isAdmin: false, type: 'voice', cTime: new Date().getHours() + ':' + new Date().getMinutes(), link: res });
                            onlineChatObj.uploadVoiceForUser(res);
                        }
                    });
                } else {
                    onlineChatObj.pendingUploadVoice = postFormData;
                    onlineChatObj.getUserIdAndUploadVoice();
                }
            }.bind({ curThis: this.curThis });
            mediaRecorder.ondataavailable = function (e) {
                this.curThis.chunks.push(e.data);
            }.bind({ curThis: this.curThis});
            mediaRecorder.start();
        }.bind({ curThis: this }), function (err) {
            this.curThis.newNotification({ isError: true, message: err });
        }.bind({ curThis: this }));
    };

    onlineChatObj.stopAndPostRecordAudio = function () {
        if (this.mediaRecorder) {
            this.mediaRecorder.stop();
        }
    }

    onlineChatObj.uploadVoiceForAdminUrlFirst = function (userId) {
        var postFormData = this.pendingUploadVoice;
        postFormData.append('userId', userId);
        postForm('/WebMainAdmin/OnlineSupport/UploadNewVoiceForOnlineChat', postFormData, function (res) {

            if (res && (typeof res === 'string' || res instanceof String)) {

                onlineChatObj.addNewMessage({ isAdmin: false, type: 'voice', cTime: new Date().getHours() + ':' + new Date().getMinutes(), link: res });
                onlineChatObj.uploadVoiceForAdmin(res);
            }
        });
    }
}

function removeTags(inputStr) {
    //return $('<div>' + inputStr +'</div>').text();
    return (inputStr + '').replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/'/g, '&#39;')
        .replace(/"/g, '&#34;');
}

function initAutoHeightForInputText(onlineChatObj) {
    var newId = uuidv4RemoveDash();
    var sQuiry = $(onlineChatObj).find('.ocUserInputTextBox');
    sQuiry[0].inputAutoHeightId = newId;
    var inputWidth = sQuiry.width();
    $('body').append('<div style="height:0px;width:0px;overflow:hidden;"><div style="-ms-word-break: break-all;word-break: break-all;padding:2px;line-height:26px;width:' + inputWidth + 'px" id="' + newId + '"></div></div>');
    sQuiry.keyup(function (e) {
        var curValue = $(this).val();
        curValue = curValue + e.key;
        if (curValue) {
            curValue = removeTags(curValue);
            curValue = curValue.replace(/\n/g, '<br />');
            var inputAutoHeightId = $(this)[0].inputAutoHeightId;
            $('#' + inputAutoHeightId).html(curValue);
            var curHeight = $('#' + inputAutoHeightId).height();
            onlineChatObj.updateInputHeight(curHeight);
        }
    });
}

function initOnlineChantUserTyping(onlineChatObj) {
    onlineChatObj.showUserTyping = function () {
        if ($(this).find('.userTypingMessage').length == 0) {
            $(this).find('.onlineChatMessages').append(`
 <span class="onlineChatMessageItem makeAdminMessage">
        <span class=" userIcon" ><span class="fa fa-user" ></span></span>
        <span style="text-align:left;" class="userMessage userTypingMessage"><span style="background-color:white;"><span style="animation-delay:0.4s" ></span><span style="animation-delay:0.3s" ></span><span style="animation-delay:0.2s"></span><span style="animation-delay:0.1s"></span></span></span>
        <span style="visibility: hidden" class="userTime" ></span>
    </span>
`);
            this.moveScrollToBottom();
        }
        clearTimeout(onlineChatObj.clearUserTypingInterval);
        onlineChatObj.clearUserTypingInterval = setTimeout(function () { this.curThis.removeUserTyping(); }.bind({ curThis: onlineChatObj }), 3000);

    }

    onlineChatObj.removeUserTyping = function () {
        if ($(this).find('.userTypingMessage').length > 0)
            $(this).find('.userTypingMessage').closest('.onlineChatMessageItem').remove();
    }
}

function initOnlineChantInitMessages(onlineChatObj) {
    onlineChatObj.addGroupMessages = function (items) {
        if (items && items.length > 0) {
            this.showUserTyping();
            setTimeout(function () {
                this.onlineChatObj.addGroupMessage(this.items);
            }.bind({ onlineChatObj: onlineChatObj, items: items }), 500);
        }
    };
    onlineChatObj.addGroupMessage = function (items) {
        if (items && items.length > 0) {
            var newItems = items.splice(1);
            this.addNewMessage(items[0]);
            this.addGroupMessages(newItems);
        }
    }
    onlineChatObj.bindLikeOrDisLikeButtonEvent = function () {
        var sQuiry = $(this).find('.onlineChatMessages');
        var lastItem = sQuiry.find('.onlineChatMessageItem:last-child');
        if (lastItem.find('.likeOrDislikeMessageButton').length > 0) {
            lastItem.find('.likeOrDislikeMessageButton').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var sQuiry = $(this).closest('.holderLikeDislikeButton');
                if (!sQuiry.hasClass('likeButtonSelectedSelected')) {
                    var isLike = $(this).hasClass('likeButton');
                    $(this).addClass('showJustThisBUtton');
                    sQuiry.addClass('likeButtonSelectedSelected');
                    $(this).closest('.likeOrDislikeCtrl').css('right', '0px');
                    $(this).closest('.onlineChat')[0].likeAnswer(Number.parseInt($(this).closest('.likeOrDislikeCtrl').attr('data-id')), isLike);
                }
            });
        }
    }
    onlineChatObj.moveScrollToBottom = function () {
        var sQuiry = $(this).find('.onlineChatMessages');
        sQuiry.animate({ scrollTop: sQuiry[0].scrollHeight }, 400);
        var lastItem = sQuiry.find('.onlineChatMessageItem:last-child');
        if (lastItem.find('.userTypingMessage').length > 0) {
            lastItem = lastItem.prev();
        }
        if (lastItem.hasClass('makeAdminMessage') && !lastItem.hasClass('makeMessToButton') && lastItem.prev().hasClass('makeAdminMessage')) {
            lastItem.prev().find('.userIcon').css('visibility', 'hidden');
        }
        var temp1 = (lastItem.attr('class') + '').replace('messagePaddingTop', '').replace(/  /g, ' ').trim();
        var temp2 = (lastItem.prev().attr('class') + '').replace('messagePaddingTop', '').replace(/  /g, ' ').trim();
        if (lastItem.prev().find('.likeOrDislikeCtrl').length > 0 || lastItem.prev().find('.yesOrNoCtrl').length > 0 || (temp1 && temp2 && temp1 != temp2)) {
            lastItem.addClass('messagePaddingTop');
        }
    };
    onlineChatObj.addNewMessage = function (msgObj) {
        if (msgObj) {
            if (msgObj.type == 'text') {
                this.removeUserTyping();
                this.addTextMessage(msgObj);
            } else if (msgObj.type == 'button') {
                this.removeUserTyping();
                this.addButtonQuestion(msgObj);
                this.moveScrollToBottom(true);
            } else if (msgObj.type == 'link') {
                this.removeUserTyping();
                this.addLink(msgObj);
                this.bindLikeOrDisLikeButtonEvent();
                this.moveScrollToBottom();
            } else if (msgObj.type == 'loginQuestion') {
                this.removeUserTyping();
                this.pintUserLoginQuestion(msgObj);
                this.moveScrollToBottom();
            } else if (msgObj.type == 'file') {
                this.removeUserTyping();
                this.addFile(msgObj);
                this.moveScrollToBottom();
            } else if (msgObj.type == 'map') {
                this.removeUserTyping();
                this.addMap(msgObj);
                this.moveScrollToBottom();
            } else if (msgObj.type == 'voice') {
                this.removeUserTyping();
                this.addVoice(msgObj);
                this.moveScrollToBottom();
            }

        }
    };
    onlineChatObj.addLink = function (msgObj) {
        $(this).find('.onlineChatMessages').append(`
    <a style="padding-top:30px;" target="_blank" href="${msgObj.link}" class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')} ">
        <span class=" userIcon" ></span>
        <span class="userMessage">
            <span style="padding:0px;">
                <span class="linkTitleDescriptionH">
                    <span class="linkTitle"><span class="fa fa-external-link ocExtenalLink"></span> ${msgObj.title}</span>
                    <span class="linkDescription">${msgObj.message + (msgObj.hasLike ? this.getLikeDislikeButtonTemplate(msgObj) : '')}</span>
                </span>
            </span>
        </span>
        <span class="userTime" ></span>
    </a>
`);
    };
    onlineChatObj.addButtonQuestion = function (msgObj) {
        $(this).find('.onlineChatMessages').append(`
    <span ${(msgObj.id ? ('data-id="' + msgObj.id + '"') : '')} data-msg="${msgObj.message}" class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : 'makeUserButton')} makeMessToButton">
        <span class=" userIcon" ></span>
        <span class="userMessage"><span style="font-weight:bold;" >${msgObj.message}</span></span>
        <span class="userTime" ></span>
    </span>
`);
        if (msgObj.isAdmin) {
            var curThis = onlineChatObj;
            $(this).find('.onlineChatMessages').find('.onlineChatMessageItem:last-child').click(function () {
                curThis.addNewMessage({ type: 'button', message: $(this).attr('data-msg'), cTime: new Date().getHours() + ':' + new Date().getMinutes(), isAdmin: false });
                curThis.showUserTyping();
                curThis.selectGroup(Number.parseInt($(this).attr('data-id')));
            });
        }
    };
    onlineChatObj.getLikeDislikeButtonTemplate = function (msgObj) {
        return `
        <span data-id="${msgObj.id}" class="likeOrDislikeCtrl" >
            <span class="holderLikeDislikeButton">
                <span class="likeOrDislikeMessageButton likeButton fa fa-thumbs-up" ></span>
                <span class="likeOrDislikeMessageButton dislikeButton fa fa-thumbs-down" ></span>
            </span>
            <span class="likeOrDislikeCtrlTitle" >آیا این پاسخ مفید بوده است؟</span>
        </span>
`
    }
    onlineChatObj.addTextMessage = function (msgObj) {
        if (msgObj && msgObj.message) {
            $(this).find('.onlineChatMessages').append(this.getTextMessageTemplate(msgObj));
            this.moveScrollToBottom();
            this.bindLikeOrDisLikeButtonEvent();
        }
    }
    onlineChatObj.getTextMessageTemplate = function (msgObj) {

        return `
    <span class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')}">
        <span class=" userIcon" ><span class="fa fa-user" ></span></span>
        <span class="userMessage"><span>${removeTags(msgObj.message) + (msgObj.hasLike ? this.getLikeDislikeButtonTemplate(msgObj) : '')}</span></span>
        <span class="userTime" >${msgObj.cTime}</span>
    </span>
`;
    }
}

function initOnlineChantInitCancellButton(onlineChatObj) {
    $(onlineChatObj).find('.cancellAllMenus').click(function () { $(this).closest('.onlineChat').find('.onlineChatShadowBox').click(); })
}

function initOnlineChantRigesterEmail(onlineChatObj) {
    onlineChatObj.openRegisterEmail = function () {
        this.closeMainMenue('.mainMenue');
        setTimeout(function () { this.curThis.openMainMenue('.emailMenue'); }.bind({ curThis: this }), 200);
        $(this).find('.emailMenue input[type=text]').val('');
    }
    $(onlineChatObj).find('.rigisterEmail').click(function () { $(this).closest('.onlineChat')[0].openRegisterEmail(); });
    $(onlineChatObj).find('.emailMenue').find('.moveBackButtonForRigisterMobile').click(function () {
        $(this).closest('.onlineChat')[0].closeMainMenue('.emailMenue');
        setTimeout(function () { this.curThis.openMainMenue('.mainMenue'); }.bind({ curThis: $(this).closest('.onlineChat')[0] }), 200);
    });
    $(onlineChatObj).find('.emailMenue').find('.mainBtn').click(function () {
        var onlineChatObj = $(this).closest('.onlineChat')[0];
        var inputEmail = $(this).closest('.emailMenue').find('input[type=text]').val();
        onlineChatObj.subScribeEmail(inputEmail);
    });
}

function initOnlineChantShowMapMenu(onlineChatObj, isAdmin) {
    onlineChatObj.openMap = function () {
        this.closeMainMenue('.mainMenue');
        setTimeout(function () { this.curThis.openMainMenue('.sendMapMenu'); setTimeout(function () { executeArrFunctions(); }, 500); }.bind({ curThis: this }), 200);
        var quiryS = $(this).find('.sendMapMenu .inputMapHolder');
        if (quiryS.find('.mapCtrl').length == 0) {
            quiryS.html(getMapTemplate(
                {
                    "parentCL": "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
                    "names": {
                        "lat": "mapLat",
                        "lon": "mapLon",
                        "zoom": "mapZoom"
                    },
                    "width": "100%",
                    "height": "260px",
                    "type": "map",
                    "label": "لطفا موقعیت را انتخاب کنید"
                }
            ));
        }
    }
    onlineChatObj.addMap = function (msgObj) {
        var fuTemplate = getMapTemplate(
            {
                "parentCL": "col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12",
                "names": {
                    "lat": "mapLat",
                    "lon": "mapLon",
                    "zoom": "mapZoom"
                },
                "width": "100%",
                "height": "260px",
                "type": "map",
                "label": "",
                "readonly": true
            });
        $(this).find('.onlineChatMessages').append(`
         <span class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')}">
            <span class=" userIcon" ><span class="fa fa-user" ></span></span>
            <span class="userMessage"><span style="display:block;width:100%;" >${fuTemplate}</span>
        </span>
`);
        executeArrFunctions();
        var curFormHolder = $(this).find('.onlineChatMessageItem:last-child');
        bindForm(msgObj, curFormHolder);
    };
    $(onlineChatObj).find('.sendMapMenu .mainBtn').click(function () {
        var formHolder = $(this).closest('.sendMapMenu');
        var formData = getFormData(formHolder);
        if (!formData.get('mapLat') || !formData.get('mapLon') || !formData.get('mapZoom')) {
            onlineChatObj.newNotification({ message: 'لطفا ابتدا مختصات خود را انتخاب کنید', isError: true });
        } else {
            onlineChatObj.closeAllMenus();
            var postData = {
                isAdmin: false,
                type: 'map',
                cTime: new Date().getHours() + ':' + new Date().getMinutes(),
                mapLat: formData.get('mapLat'),
                mapLon: formData.get('mapLon'),
                mapZoom: formData.get('mapZoom'),
            };
            onlineChatObj.addNewMessage(postData);
            if (isAdmin)
                onlineChatObj.sendMapForAdmin(postData);
            else
                onlineChatObj.sendMapForUser(postData);
        }
    });
    $(onlineChatObj).find('.sendMap').click(function () { $(this).closest('.onlineChat')[0].openMap(); });
    $(onlineChatObj).find('.sendMapMenu').find('.moveBackButtonForRigisterMobile').click(function () {
        $(this).closest('.onlineChat')[0].closeMainMenue('.sendMapMenu');
        setTimeout(function () { this.curThis.openMainMenue('.mainMenue'); }.bind({ curThis: $(this).closest('.onlineChat')[0] }), 200);
    });
}

function initOnlineChantUploadNewFile(onlineChatObj, isAdmin) {
    onlineChatObj.openUploadFile = function () {
        this.closeMainMenue('.mainMenue');
        if (!isUserLogin)
            onlineChatObj.askUserForLogin();
        else
            setTimeout(function () { this.curThis.openMainMenue('.uploadFile'); }.bind({ curThis: this }), 200);
    }
    $(onlineChatObj).find('.fa-camera').click(function ()
    {
        $(this).closest('.onlineChat')[0].openUploadFile();
    });
    $(onlineChatObj).find('.fa-paperclip').click(function () {
        $(this).closest('.onlineChat')[0].openUploadFile();
    });
    onlineChatObj.uploadFileForAdmin = function (userid) {
        var formData = this.pendingUploadFile;
        if (formData) {
            formData.append('userId', userid);
            postForm('/WebMainAdmin/OnlineSupport/UploadNewFileForOnlineChat', formData, function (res) {
                if (res && (typeof res === 'string' || res instanceof String)) {
                    onlineChatObj.closeAllMenus();
                    onlineChatObj.addNewMessage({ isAdmin: false, type: 'file', cTime: new Date().getHours() + ':' + new Date().getMinutes(), link: res });
                    onlineChatObj.uploadFileAdmin(res);
                }
            });
        }
    }

    onlineChatObj.addFile = function (msgObj) {
        $(this).find('.onlineChatMessages').append(
            `
    <span class="onlineChatMessageItem ${(msgObj.isAdmin ? 'makeAdminMessage' : '')}">
        <span class=" userIcon" ><span class="fa fa-user" ></span></span>
        <span class="userMessage"><span><a target="_blank" class="onDownloadButton" href="${msgObj.link}">دانلود</a> </span></span>
        <span class="userTime" >${msgObj.cTime}</span>
    </span>
`);
    };
    $(onlineChatObj).find('.uploadNewFile').click(function () { $(this).closest('.onlineChat')[0].openUploadFile(); });
    $(onlineChatObj).find('.uploadFile').find('.moveBackButtonForRigisterMobile').click(function () {
        $(this).closest('.onlineChat')[0].closeMainMenue('.uploadFile');
        //setTimeout(function () { this.curThis.openMainMenue('.mainMenue'); }.bind({ curThis: $(this).closest('.onlineChat')[0] }), 200);
    });
    $(onlineChatObj).find('.uploadFile').find('input[type=file]').change(function () {
        var formData = getFormData($(this).closest('.uploadFile'));
        var onlineChatObj = $(this).closest('.onlineChat')[0];
        if (!isAdmin) {
            postForm('/Home/UploadNewFileForOnlineChat', formData, function (res) {
                if (res && (typeof res === 'string' || res instanceof String)) {
                    onlineChatObj.closeAllMenus();
                    onlineChatObj.addNewMessage({ isAdmin: false, type: 'file', cTime: new Date().getHours() + ':' + new Date().getMinutes(), link: res });
                    onlineChatObj.uploadFileUser(res);
                }
            });
        } else {
            onlineChatObj.getUserIdAndUploadFile(formData);
        }
    });
}

function initOnlineChantNotification(onlineChatObj) {
    onlineChatObj.notifiCount = 0;
    onlineChatObj.newNotification = function (nObj) {
        var curElement = $(this)[0];
        if ($(this).find('.onlineChatNotificationHolder').length == 0) {
            $(this).append('<span class="onlineChatNotificationHolder" ></span>');
        }
        curElement.notifiCount = curElement.notifiCount + 1;
        var tempStr = '<span id="nId_' + curElement.notifiCount + '" class="onlineChatNotification ' + (nObj.isError ? 'noError' : '') + '" >' + nObj.message + '</span>';
        curElement.hideNotification(curElement.notifiCount);
        $(this).find('.onlineChatNotificationHolder').append(tempStr);
        setTimeout(function () { $('#nId_' + curElement.notifiCount).addClass('onlineChatNotificationShow'); }, 1);
    };
    onlineChatObj.hideNotification = function (nId) {
        setTimeout(function () { $('#nId_' + this.nId).removeClass('onlineChatNotificationShow') }.bind({ nId: nId }), 3000);
    };
}

function initOnlineChantHeaderButtons(onlineChatObj) {
    onlineChatObj.openMainMenue = function (targetMenu) {
        $(this).addClass('openMainMenue');
        $(this).find(targetMenu).css('bottom', '0px');
    };
    onlineChatObj.closeMainMenue = function (targetMenu) {
        $(this).removeClass('openMainMenue');
        $(this).find(targetMenu).css('bottom', '-500px');
    };
    onlineChatObj.close = function () {
        if (this.connection) {
            this.connection.stop();
            this.connection.dontTryAgin = true;
        }
        $(this).remove();
    }
    onlineChatObj.closeAllMenus = function () {
        this.closeMainMenue('.mainMenue');
        this.closeMainMenue('.mobileMenue');
        this.closeMainMenue('.emailMenue');
        this.closeMainMenue('.uploadFile');
        this.closeMainMenue('.sendMapMenu');
    }
    $(onlineChatObj).find('.onlineChatHeaderActionButtonClose').click(function () {
        $(this).closest('.onlineChat')[0].close();
    });
    $(onlineChatObj).find('.onlineChatHeaderMenueButton').click(function () {
        $(this).closest('.onlineChat')[0].openMainMenue('.mainMenue');
    });
    $(onlineChatObj).find('.onlineChatShadowBox').click(function () {
        var curElement = $(this).closest('.onlineChat')[0];
        curElement.closeAllMenus();
    });
}

function initOnlineChantActiveAndDeactiveNotifySound(onlineChatObj) {
    onlineChatObj.enableDisableNotifiSound = function () {
        if (!onlineChatObj.isSoundEnabled) {
            onlineChatObj.isSoundEnabled = true;
            $(this).find('.enableDisableNotifiSound').html('<span class="fa fa-volume"></span>غیر فعال کردن صدا');
            onlineChatObj.newNotification({ message: 'صدا فعال شد' });
        } else {
            onlineChatObj.isSoundEnabled = false;
            $(this).find('.enableDisableNotifiSound').html('<span class="fa fa-volume-mute"></span>فعال کردن صدا');
            onlineChatObj.newNotification({ message: 'صدا غیر فعال شد' });
        }
    };
    $(onlineChatObj).find('.enableDisableNotifiSound').click(function () { var ctrl = $(this).closest('.onlineChat')[0]; ctrl.enableDisableNotifiSound(); ctrl.closeMainMenue('.mainMenue'); });
}