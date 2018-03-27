$(function () {

    /*********************************************全局变量********************************************/
    App = {
        rand: 0,
        countdown: 90,
        bs: 0,
        page: 0,
        ct: 0,
        bhy: false
    }
    /*********************************************全局变量********************************************/
    var completed = 0, total = $('section img').length;
    ////图片资源初始化
    function imginit() {
        /* 接下来我们处理图片预加载，并且和进度有序的组织起来 */
        $('section').imagesLoaded().done(function (instance) {
            if ($('#fopenid').html() === $('#openid').html()) {
                if ($('#cs').html() * 1 > 0) {
                    if ($('#cs').html() * 1 < 3) {
                        $('#page_cj_btn').hide();
                        $('#page_cj_jp').show();
                        $('#page_cj_cs').html("您还有" + ($('#cs').html() * 1) + "次机会");
                    }
                    $('#sc').show();
                    $('#load').css({
                        'background': 'url(../../../img/fm.jpg) bottom',
                        'background-size': 'cover'
                    });
                    $('#load_m').hide();
                }
                else {
                    Max();
                    App.ct = 1;
                }
            }
            else {
                MaxF();
            }
            getOwnID();
        }).progress(function (instance, image) {
            completed++; //添加计数器
            var imgprogress = completed / total; //生成进度数值
            $("#load_n").text(parseInt(imgprogress * 100) + "%");
            $('#load_jdt').css('width', imgprogress * 342 + 'px');
        });
    }
    imginit();

    /*********************************************内部函数********************************************/

    function settime(val) {
        if (App.countdown == 0) {

            App.countdown = 60;
        } else {
            App.countdown--;
            $('#page_zc_sms').html(App.countdown + "S");
            setTimeout(function () {
                settime(val)
            }, 1000)
        }

    }

    //获取url中的参数
    function getUrlParam(url, name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = url.match(reg);  //匹配目标参数
        if (r != null) return decodeURI(r[2]); return null; //返回参数值
    }

    function getUserlist() {

        $.ajax({
            //提交数据的类型 POST GET
            type: "POST",
            //提交的网址
            url: "/Home/Flist",
            cache: false,
            async: true,
            //提交的数据
            data: {
                "openid": $("#openid").html().trim()
            },
            //返回数据的格式
            datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
            //成功返回之后调用的函数
            success: function (data) {
                if (data != null) {
                    if (data.length >= 21) {
                        $('.page_l_jdt_ov').css('width', '505px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.7).toFixed(1) + '.png');

                    }
                    else if (data.length >= 20) {
                        $('.page_l_jdt_ov').css('width', '479px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');
                    }
                    else if (data.length >= 19) {
                        $('.page_l_jdt_ov').css('width', '456px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');
                    }
                    else if (data.length >= 18) {
                        $('.page_l_jdt_ov').css('width', '433px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');

                    }
                    else if (data.length >= 17) {
                        $('.page_l_jdt_ov').css('width', '407px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 16) {
                        $('.page_l_jdt_ov').css('width', '384px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 15) {
                        $('.page_l_jdt_ov').css('width', '361px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 14) {
                        $('.page_l_jdt_ov').css('width', '334px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 13) {
                        $('.page_l_jdt_ov').css('width', '311px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 12) {
                        $('.page_l_jdt_ov').css('width', '288px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 11) {
                        $('.page_l_jdt_ov').css('width', '261px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 10) {
                        $('.page_l_jdt_ov').css('width', '238px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 9) {
                        $('.page_l_jdt_ov').css('width', '215px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 8) {
                        $('.page_l_jdt_ov').css('width', '188px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 7) {
                        $('.page_l_jdt_ov').css('width', '165px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 6) {
                        $('.page_l_jdt_ov').css('width', '142px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 5) {
                        $('.page_l_jdt_ov').css('width', '116px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');

                    }
                    else if (data.length >= 4) {
                        $('.page_l_jdt_ov').css('width', '93px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');

                    }
                    else if (data.length >= 3) {
                        $('.page_l_jdt_ov').css('width', '70px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');
                    }
                    else if (data.length >= 2) {
                        $('.page_l_jdt_ov').css('width', '46px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');

                    }
                    else if (data.length >= 1) {
                        $('.page_l_jdt_ov').css('width', '23px');
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');

                    }
                    else {
                        $('#page_l_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');

                    }
                    var dom = "";
                    for (var i = 0; i < data.length; i++) {
                        dom += '<div class="page_l_flist_op">';
                        dom += '<div class="page_l_flist_ops">';
                        dom += '<img src="' + data[i].HeadUrl + '" />';
                        dom += '</div>';
                        dom += '<div class="page_l_flist_op_c">' + ((data[i].NickName).length > 3 && (data[i].NickName).substring(0, 3)+"..." || data[i].NickName) + '</div>';
                        dom += '<div class="page_l_flist_op_r">已为您提高倍数</div>';
                        dom += '</div>';
                    }
                    if (dom != '') {
                        $('.page_l_flist').html(dom);
                    }
                    //$('#sc').show();
                    //$('#load').css({
                    //    'background': 'url(../../../img/fm.jpg) top center',
                    //    'background-size': 'cover'
                    //});
                    //$('#load_m').hide();
                    $('#load').hide();
                    $('#fvideo').hide();
                    $('#page_l').show();
                    if (data.length > 0) {
                        $("#page_l_czx").show();
                        $("#page_l_cz").hide();
                    }
                }
                else {

                }
            },
            //调用出错执行的函数
            error: function (e) {
            }
        });
    }
    function Max() {
        $.ajax({
            //提交数据的类型 POST GET
            type: "POST",
            //提交的网址
            url: "/Home/Max",
            cache: false,
            async: true,
            //提交的数据
            data: {
                "openid": $("#openid").html().trim()
            },
            //返回数据的格式
            datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
            //成功返回之后调用的函数
            success: function (data) {
                App.bs = data * 1;
                var dom = '';
                dom += '<tr>';
                dom += '<td class="light">' + data + '倍</td>';
                dom += '<td>' + (data * 1 + 0.1).toFixed(1) + '</td>';
                dom += '<td>' + (data * 1 + 0.2).toFixed(1) + '</td>';
                dom += '<td>' + (data * 1 + 0.3).toFixed(1) + '</td>';
                dom += '<td>' + (data * 1 + 0.4).toFixed(1) + '</td>';
                dom += '<td>' + (data * 1 + 0.5).toFixed(1) + '</td>';
                dom += '<td>' + (data * 1 + 0.6).toFixed(1) + '</td>';
                dom += '<td class="light">' + (data * 1 + 0.7).toFixed(1) + '倍</td>';
                dom += '</tr>';
                $('.page_l_jdt_bs').html(dom);
                getUserlist();
            },
            //调用出错执行的函数
            error: function (e) {
            }
        });
    }

    //获取好友最高倍数
    function MaxF() {

        $.ajax({
            //提交数据的类型 POST GET
            type: "POST",
            //提交的网址
            url: "/Home/GetUserinfoF",
            cache: false,
            async: true,
            //提交的数据
            data: {
                "openid": $("#fopenid").html().trim()
            },
            //返回数据的格式
            datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
            //成功返回之后调用的函数
            success: function (data) {
                if (data != null) {
                    App.bs = data.BaseMultiple * 1;
                    $('#page_f_head_imgs').attr('src', data.HeadUrl);
                    $('#page_f_head_name').html(data.NickName);
                    var dom = '';
                    dom += '<tr>';
                    dom += '<td class="light">' + App.bs + '倍</td>';
                    dom += '<td>' + (App.bs * 1 + 0.1).toFixed(1) + '</td>';
                    dom += '<td>' + (App.bs * 1 + 0.2).toFixed(1) + '</td>';
                    dom += '<td>' + (App.bs * 1 + 0.3).toFixed(1) + '</td>';
                    dom += '<td>' + (App.bs * 1 + 0.4).toFixed(1) + '</td>';
                    dom += '<td>' + (App.bs * 1 + 0.5).toFixed(1) + '</td>';
                    dom += '<td>' + (App.bs * 1 + 0.6).toFixed(1) + '</td>';
                    dom += '<td class="light">' + (App.bs * 1 + 0.7).toFixed(1) + '倍</td>';
                    dom += '</tr>';
                    $('.page_l_jdt_bs').html(dom);
                    getUserlistF();
                }

            },
            //调用出错执行的函数
            error: function (e) {
            }
        });
    }
    //获取好友的帮助列表
    function getUserlistF() {
        $.ajax({
            //提交数据的类型 POST GET
            type: "POST",
            //提交的网址
            url: "/Home/Flist",
            cache: false,
            async: true,
            //提交的数据
            data: {
                "openid": $("#fopenid").html().trim()
            },
            //返回数据的格式
            datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
            //成功返回之后调用的函数
            success: function (data) {
                if (data != null) {
                    if (data.length >= 21) {
                        $('.page_l_jdt_ov').css('width', '505px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.7).toFixed(1) + '.png');

                    }
                    else if (data.length >= 20) {
                        $('.page_l_jdt_ov').css('width', '479px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');

                    }
                    else if (data.length >= 19) {
                        $('.page_l_jdt_ov').css('width', '456px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');

                    }
                    else if (data.length >= 18) {
                        $('.page_l_jdt_ov').css('width', '433px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.6).toFixed(1) + '.png');

                    }
                    else if (data.length >= 17) {
                        $('.page_l_jdt_ov').css('width', '407px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 16) {
                        $('.page_l_jdt_ov').css('width', '384px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 15) {
                        $('.page_l_jdt_ov').css('width', '361px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.5).toFixed(1) + '.png');

                    }
                    else if (data.length >= 14) {
                        $('.page_l_jdt_ov').css('width', '334px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 13) {
                        $('.page_l_jdt_ov').css('width', '311px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 12) {
                        $('.page_l_jdt_ov').css('width', '288px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.4).toFixed(1) + '.png');

                    }
                    else if (data.length >= 11) {
                        $('.page_l_jdt_ov').css('width', '261px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 10) {
                        $('.page_l_jdt_ov').css('width', '238px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 9) {
                        $('.page_l_jdt_ov').css('width', '215px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.3).toFixed(1) + '.png');

                    }
                    else if (data.length >= 8) {
                        $('.page_l_jdt_ov').css('width', '188px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 7) {
                        $('.page_l_jdt_ov').css('width', '165px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 6) {
                        $('.page_l_jdt_ov').css('width', '142px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.2).toFixed(1) + '.png');

                    }
                    else if (data.length >= 5) {
                        $('.page_l_jdt_ov').css('width', '116px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');

                    }
                    else if (data.length >= 4) {
                        $('.page_l_jdt_ov').css('width', '93px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');

                    }
                    else if (data.length >= 3) {
                        $('.page_l_jdt_ov').css('width', '70px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + (App.bs * 1 + 0.1).toFixed(1) + '.png');
                    }
                    else if (data.length >= 2) {
                        $('.page_l_jdt_ov').css('width', '46px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');
                    }
                    else if (data.length >= 1) {
                        $('.page_l_jdt_ov').css('width', '23px');
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');
                    }
                    else {
                        $('#page_f_bs').attr('src', '../../../img/page_l_' + App.bs + '.png');

                    }
                    var dom = "";
                    for (var i = 0; i < data.length; i++) {
                        dom += '<div class="page_l_flist_op">';
                        dom += '<div class="page_l_flist_ops">';
                        dom += '<img src="' + data[i].HeadUrl + '" />';
                        dom += '</div>';
                        dom += '<div class="page_l_flist_op_c">' + ((data[i].NickName).length > 3 && (data[i].NickName).substring(0, 3) + "..." || data[i].NickName)+ '</div>';
                        dom += '<div class="page_l_flist_op_r">已为TA提高倍数</div>';
                        dom += '</div>';
                    }
                    if (dom != '') {
                        $('.page_l_flist').html(dom);
                    }
                    $('#load').remove();
                    $('#fvideo').remove();
                    $('#page_f').show();
                }
            },
            //调用出错执行的函数
            error: function (e) {
            }
        });
    }

    function getOwnID() {
        $.ajax({
            //提交数据的类型 POST GET
            type: "POST",
            //提交的网址
            url: "/Home/GetOwnUserid",
            cache: false,
            async: true,
            //提交的数据
            data: {
                "openid": $("#openid").html().trim()
            },
            //返回数据的格式
            datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
            //成功返回之后调用的函数
            success: function (data) {
                if (data != "0" && data != '') {
                    $('#userid').html(data);
                }
            },
            //调用出错执行的函数
            error: function (e) {
            }
        });
    }
    /*********************************************内部函数********************************************/
    $('#sc').click(function () {
        $('#load').hide();
        document.getElementById('video').play();
    });
    $('#tiaoguo').click(function () {
        document.getElementById('video').pause();
        if (App.ct == 1) {
            $('#page_l').show();
        } else {
            $('#page_cj').show();
        }
        $('#fvideo').remove();
    });
    $('#video').on('ended', function () {
        $('#fvideo').remove();

        if (App.ct == 1) {
            $('#page_l').show();
        }
        else {
            $('#page_cj').show();
        }
    })
    $('#page_share').click(function () {
        $('#page_share').hide();
    });
    $('#page_l_yq').click(function () {
        $('#page_share').show();
    });
    //活动说明
    $('#page_cj_cjsm_btn').click(function () {
        $('#page_gz').show();
    });
    $('#page_cj_cjpasm_btn').click(function () {
        $('#page_pa').show();
    });
    $('.page_l_cjsm').click(function () {
        $('#page_cj_cjsm_btn').trigger('click');
    });
    $('.page_l_cjpasm').click(function () {
        $('#page_cj_cjpasm_btn').trigger('click');
    });
    //关闭活动说明
    $('#page_gz_btn').click(function () {
        $('#page_gz').hide();
    });
    $('#page_pa_btn').click(function () {
        $('#page_pa').hide();
    });
    //抽奖
    var cjkg = true;//开关
    $('#page_cj_btn').click(function () {
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/Lottery",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    openid: $("#openid").html().trim(),
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {
                    var msg = data.split(',');
                    if (msg[1] > 0) {
                        $('#page_cj_jp').removeClass('bounceOutUp').addClass('bounceInDown');
                        $('#page_cj_jp_img').attr('src', '../../../img/page_cj_' + msg[0] + '.png');
                        $('#page_cj_btn').hide();
                        $('#page_cj_jp').show();
                        var cs = $('#cs').html() * 1;
                        if (cs > 0) {
                            $('#page_cj_cs').html("您还有" + (cs - 1) + "次机会");
                            $('#cs').html(cs - 1);
                        }
                        if (cs == 1) {
                            setTimeout(function () {
                                $('#ax').show();
                            }, 1000);
                            setTimeout(function () {
                                $('#ax').hide();
                            }, 2000);
                            $('#page_cj_btn2').show();
                            $('#page_cj_btn1').hide();
                        }

                    }
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }
    });
    //再抽一次
    $('#page_cj_btn1').click(function () {
        $('#page_cj_jp').removeClass('bounceInDown').addClass('bounceOutUp');
        setTimeout(function () {
            $('#page_cj_btn').trigger('click');
        }, 1000);

    });
    //最后返现倍数
    $('#page_cj_btn2').click(function () {
        $('#page_l').show();
        $('#page_cj_jp').hide();
        Max();
    });
    //初始倍数直接冲
    $('#page_l_cz').click(function () {
        var uid = $('#userid').html().trim();
        if (uid != '') {
            window.location.href = $('#url').html();
            return;
        }
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/Imgy",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    openid: $("#openid").html().trim(),
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {
                    $('#page_l').hide();
                    $('#page_zc').show();
                    $('#page_zc_imgyzm').attr('src', data);
                    App.rand = data.split('rand=')[1];
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }



    });
    $('#page_l_czx').click(function () {
        $('#page_l_cz').trigger('click');
    });
    $('#page_zc_imgyzm').click(function () { $('#page_l_cz').trigger('click'); });
    //获取短信验证码
    $('#page_zc_sms').click(function () {
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/SendSmsCode",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    "cellphone": $('#page_zc_phone').val().trim(),
                    "rand": App.rand,
                    "captcha": $('#page_zc_yzm').val().trim(),
                    "validateType": 1
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {
                    if (data == "true") {
                        settime(90);
                    }
                    else {
                        alert(data);
                    }
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }
    });
    //注册
    $('#page_res_zc').click(function () {
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/Regist",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    "cellphone": $('#page_zc_phone').val().trim(),
                    "password": $('#page_zc_pwd').val().trim(),
                    "smscode": $('#page_zc_syzm').val().trim(),
                    "registSource": "wechat183",
                    "openid": $('#openid').html().trim(),
                    "fopenid": $('#fopenid').html().trim()
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {
                    if (data == "true") {
                        if (App.bhy) {
                            $('#page_alert_m').css({
                                'background': 'url(../../../img/alert_jb.png) top center',
                                'background-size': '100% 100%'
                            });
                            $('#page_alert').show();
                            MaxF();
                        }
                        else {
                            window.location.href = $('#url').html();
                        }

                    }
                    else {
                        alert(data);
                    }
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }

    });
    //关闭注册页
    $('#page_res_close').click(function () {
        $('#page_zc').hide();
        if (App.page > 0) {
            $('#page_f').show();
        }
        else {
            $('#page_l').show();
        }
    });
    //跳转到登录页
    $('#page_zc_dl_btn').click(function () {
        $('#page_dl').show();
        $('#page_zc').hide();
    });
    //跳转到注册页
    $('#page_dl_zc_btn').click(function () {
        $('#page_dl').hide();
        $('#page_zc').show();
    })
    //登录
    $('#page_dl_login').click(function () {
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/Login",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    "userName": $('#page_dl_phone').val().trim(),
                    "password": $('#page_dl_pwd').val().trim(),
                    "openid": $('#openid').html().trim(),
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {

                    if (data == "true") {
                        if (App.bhy) {
                            $('#page_alert_m').css({
                                'background': 'url(../../../img/alert_ord.png) top center',
                                'background-size': '100% 100%'
                            });
                            $('#page_alert').show();
                            $('#page_dl').hide();
                            return;
                        } else {
                            window.location.href = $('#url').html();
                        }

                    }
                    else {
                        alert(data);
                    }
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }

    });

    //关闭登录页
    $('#page_dl_close').click(function () {
        $('#page_dl').hide();
        if (App.page > 0) {
            $('#page_f').show();
        }
        else {
            $('#page_l').show();
        }

    });
    //注册帮助好友
    $('#page_f_btn').click(function () {
        var uid = $('#userid').html().trim();
        if (uid != '') {
            $('#page_alert_m').css({
                'background': 'url(../../../img/alert_ord.png) top center',
                'background-size': '100% 100%'
            });
            $('#page_alert').show();
            return;
        }
        if (cjkg) {
            cjkg = false;
            $.ajax({
                //提交数据的类型 POST GET
                type: "POST",
                //提交的网址
                url: "/Home/Imgy",
                cache: false,
                async: true,
                //提交的数据
                data: {
                    openid: $("#openid").html().trim(),
                },
                //返回数据的格式
                datatype: "json",//"xml", "html", "script", "json", "jsonp", "text".
                //成功返回之后调用的函数
                success: function (data) {
                    $('#page_f').hide();
                    App.page = 1;
                    App.bhy = true;
                    $('#page_zc').show();
                    $('#page_zc_imgyzm').attr('src', data);
                    App.rand = data.split('rand=')[1];
                    //$('#page_zc_dl').hide();
                    $('#page_zc_mag').css("margin-top", "60px");
                    cjkg = true;
                },
                //调用出错执行的函数
                error: function (e) {
                    cjkg = true;
                }
            });
        }
    });
    $('#page_alert').click(function () {
        $('#page_alert').hide();
        $('#page_f_btn2').show();
        $('#page_f_btn').hide();
        $('#page_zc').hide();
        if (App.bhy) {
            $('#page_f').show();

        }
    });
    $('#page_f_btn2').click(function () {
        window.location.href = "http://jumaxnew.forean.cn/home/index";
    });
    dz({
        debug: false,
        appid: 'wx3bdf51f7e21320d3',  ////公众号appid
        oauthurl: escape(window.location.href),////当前页面完整url
        timestamp: '1414587457',////生成签名的时间戳
        nonceStr: 'wm3wzytpz0wzccnw',////生成签名的随机串
        title: '充返制胜法宝！', // 分享标题
        desc: '充返制胜法宝！', // 分享描述window.location='../index.html?ti='+q.ti+'&fens='+q.fens+'&gailv='+gailv;
        link: 'http://jumaxnew.forean.cn/home/index/' + $("#openid").html().trim() + "/" + $("#openid").html().trim(), // 分享链接
        imgUrl: 'http://jumaxnew.forean.cn/img/fx.jpg', // 分享图标
        success: function () {////分享成功后执行的方法

        }
    })
});