namespace LogixHealth.EnterpriseLogger.Extensions
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class ClientOsBrowser
    {
        private static Dictionary<string, string> _osVersionMap = new Dictionary<string, string>
        {
            {"4.90","ME" },
            { "NT3.51","NT 3.11"},
            { "NT4.0","NT 4.0"},
            { "NT 5.0","2000"},
            { "NT 5.1","XP"},
            { "NT 5.2","XP"},
            { "NT 6.0","Vista"},
            { "NT 6.1","7"},
            { "NT 6.2","8"},
            { "NT 6.3","8.1"},
            { "NT 6.4","10"},
            { "NT 10.0","10"},
            { "ARM","RT"}
        };

        private static Dictionary<string, string> _browserVersionMap = new Dictionary<string, string>
        {
            {"/8","1.0" },
            { "/1","1.2"},
            { "/3","1.3"},
            { "/412","2.0"},
            { "/416","2.0.2"},
            { "/417","2.0.3"},
            { "/419","2.0.4"},
            {"?","/" }
        };

        public ClientOsBrowser(string userAgent)
        {
            bool matchFound = false;

            foreach (var matchItem in _clientOSMatchs)
            {
                foreach (var regexItem in matchItem.Regexes)
                {
                    if (regexItem.IsMatch(userAgent))
                    {
                        var match = regexItem.Match(userAgent);

                        matchItem.Action(match, this);
                        matchFound = true;

                        break;
                    }
                }

                if (matchFound) break;
            }

            matchFound = false;

            foreach (var matchItem in _clientBrowserMatchs)
            {
                foreach (var regexItem in matchItem.Regexes)
                {
                    if (regexItem.IsMatch(userAgent))
                    {
                        var match = regexItem.Match(userAgent);
                        matchItem.Action(match, this);
                        this.BrowserMajorVersion = new Regex(@"\d*").Match(this.BrowserVersion).Value;
                        matchFound = true;

                        break;
                    }
                }

                if (matchFound) break;
            }
        }

        public string OSName { get; set; }

        public string OSVersion { get; set; }

        public string BrowserMajorVersion { get; set; }

        public string BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        private static void OSNameVersionAction(Match match, dynamic obj)
        {
            ClientOsBrowser current = obj as ClientOsBrowser;
            current.OSName = new Regex(@"^[a-zA-Z]+", RegexOptions.IgnoreCase).Match(match.Value).Value;
            if (match.Value.Length > current.OSName.Length)
            {
                current.OSVersion = match.Value.Substring(current.OSName.Length + 1);
            }
        }

        private static void BrowserNameVersionAction(Match match, dynamic obj)
        {
            ClientOsBrowser current = obj as ClientOsBrowser;

            current.BrowserName = new Regex(@"^[a-zA-Z]+", RegexOptions.IgnoreCase).Match(match.Value).Value;
            if (match.Value.Length > current.BrowserName.Length)
            {
                current.BrowserVersion = match.Value.Substring(current.BrowserName.Length + 1);
            }
        }

        private static List<Expression> _clientOSMatchs = new List<Expression>
        {
            // Windows (iTunes)
            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex
                    (
                        @"microsoft\s(windows)\s(vista|xp)",
                        RegexOptions.IgnoreCase
                    ),
                },
                Action = OSNameVersionAction
            },

            // Windows RT/Phone
            new Expression
            {
                Regexes = new List<Regex>
                {
                    // Windows RT
                    new Regex(@"(windows)\snt\s6\.2;\s(arm)", RegexOptions.IgnoreCase),

                    // Windows Phone
                    new Regex(@"(windows\sphone(?:\sos)*)[\s\/]?([\d\.\s]+\w)*",RegexOptions.IgnoreCase),
                },
                Action = (Match match, System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    current.OSName = new Regex(@"(^[a-zA-Z]+\s[a-zA-Z]+)",RegexOptions.IgnoreCase).Match(match.Value).Value;

                    if(current.OSName.Length<match.Value.Length)
                    {
                        var version = match.Value.Substring(current.OSName.Length+1);

                        current.OSVersion = _osVersionMap.Keys.Any(m=>m==version)? _osVersionMap[version]:version;
                    }
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(windows\smobile|windows)[\s\/]?([ntce\d\.\s]+\w)",RegexOptions.IgnoreCase)
                },
                Action = (Match match, System.Object obj)=>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    current.OSName = new Regex(@"(^[a-zA-Z]+)",RegexOptions.IgnoreCase).Match(match.Value).Value;

                    if(current.OSName.Length<match.Value.Length)
                    {
                        var version = match.Value.Substring(current.OSName.Length + 1);
                        current.OSVersion = _osVersionMap.Keys.Any(m=>m==version)? _osVersionMap[version]:version;
                    }
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(win(?=3|9|n)|win\s9x\s)([nt\d\.]+)",RegexOptions.IgnoreCase)
                },
                Action = (Match match, System.Object obj)=>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Windows";

                    var version = nameAndVersion[1];

                    current.OSVersion = _osVersionMap.Keys.Any(m=>m==version)? _osVersionMap[version]:version;
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"\((bb)(10);",RegexOptions.IgnoreCase)// BlackBerry 10
                },
                Action = (Match match, System.Object obj)=>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    current.OSName = "BlackBerry";

                    current.OSVersion = "BB10";
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(blackberry)\w*\/?([\w\.]+)*",RegexOptions.IgnoreCase),// Blackberry
                    new Regex(@"(tizen)[\/\s]([\w\.]+)",RegexOptions.IgnoreCase),// Tizen
                    new Regex(@"(android|webos|palm\sos|qnx|bada|rim\stablet\sos|meego|contiki)[\/\s-]?([\w\.]+)*",RegexOptions.IgnoreCase),// Android/WebOS/Palm/QNX/Bada/RIM/MeeGo/Contiki
                    new Regex(@"linux;.+(sailfish);",RegexOptions.IgnoreCase)// Sailfish OS
                },
                Action = OSNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(symbian\s?os|symbos|s60(?=;))[\/\s-]?([\w\.]+)*",RegexOptions.IgnoreCase)// Symbian
                },
                Action = (Match match, System.Object obj)=>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Symbian";

                    current.OSVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"\((series40);",RegexOptions.IgnoreCase)// Series 40
                },
                Action = (Match match, System.Object obj)=>{
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    current.OSName = match.Value;
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"mozilla.+\(mobile;.+gecko.+firefox",RegexOptions.IgnoreCase)// Firefox OS
                },
                Action = (Match match, System.Object obj)=>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Firefox OS";

                    current.OSVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    // Console
                    new Regex(@"(nintendo|playstation)\s([wids34portablevu]+)",RegexOptions.IgnoreCase),// Nintendo/Playstation

                    // GNU/Linux based
                    new Regex(@"(mint)[\/\s\(]?(\w+)*",RegexOptions.IgnoreCase),// Mint
                    new Regex(@"(mageia|vectorlinux)[;\s]",RegexOptions.IgnoreCase),// Mageia/VectorLinux
                    new Regex(@"(joli|[kxln]?ubuntu|debian|[open]*suse|gentoo|(?=\s)arch|slackware|fedora|mandriva|centos|pclinuxos|redhat|zenwalk|linpus)[\/\s-]?(?!chrom)([\w\.-]+)*",RegexOptions.IgnoreCase),// Joli/Ubuntu/Debian/SUSE/Gentoo/Arch/Slackware
                    
                    // Ubuntu, Linux
                    new Regex(@"(hurd|linux)\s?([\w\.]+)*",RegexOptions.IgnoreCase),// Hurd/Linux
                    new Regex(@"(gnu)\s?([\w\.]+)*",RegexOptions.IgnoreCase)// GNU
                },
                Action = OSNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(cros)\s[\w]+\s([\w\.]+\w)",RegexOptions.IgnoreCase)// Chromium OS
                },
                Action = (Match match, System.Object obj)=>{
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Chromium OS";

                    current.OSVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(sunos)\s?([\w\.]+\d)*",RegexOptions.IgnoreCase)// Solaris
                },
                Action = (Match match, System.Object obj)=>{
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Solaris";

                    current.OSVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"\s([frentopc-]{0,4}bsd|dragonfly)\s?([\w\.]+)*",RegexOptions.IgnoreCase),// FreeBSD/NetBSD/OpenBSD/PC-BSD/DragonFly
                    new Regex(@"(haiku)\s(\w+)",RegexOptions.IgnoreCase)
                },
                Action = OSNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(ip[honead]+)(?:.*os\s([\w]+)*\slike\smac|;\sopera)",RegexOptions.IgnoreCase)// iOS
                },
                Action = (Match match, System.Object obj)=>{
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "iOS";

                    current.OSVersion = new Regex(@"\d+(?:\.\d+)*").Match(nameAndVersion[1].Replace("_",".")).Value;
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(mac\sos\sx)\s?([\w\s\.]+\w)*",RegexOptions.IgnoreCase),
                    new Regex(@"(macintosh|mac(?=_powerpc)\s)",RegexOptions.IgnoreCase)// Mac OS
                },
                Action = (Match match, System.Object obj)=>{
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = new string[]{match.Value.Substring(0,match.Value.IndexOf(" ")),match.Value.Substring(match.Value.IndexOf(" ")+1) };

                    current.OSName = "Mac OS";

                    current.OSVersion = nameAndVersion[1].Replace('_','.');
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"((?:open)?solaris)[\/\s-]?([\w\.]+)*",RegexOptions.IgnoreCase),// Solaris
                    new Regex(@"(aix)\s((\d)(?=\.|\)|\s)[\w\.]*)*",RegexOptions.IgnoreCase),// AIX
                    new Regex(@"(plan\s9|minix|beos|os\/2|amigaos|morphos|risc\sos|openvms)",RegexOptions.IgnoreCase),// Plan9/Minix/BeOS/OS2/AmigaOS/MorphOS/RISCOS/OpenVMS
                    new Regex(@"(unix)\s?([\w\.]+)*",RegexOptions.IgnoreCase)// UNIX
                },
                Action = OSNameVersionAction
            }
        };

        private static List<Expression> _clientBrowserMatchs = new List<Expression>
        {
            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(opera\smini)\/([\w\.-]+)",RegexOptions.IgnoreCase),// Opera Mini
                    new Regex(@"(opera\s[mobiletab]+).+version\/([\w\.-]+)",RegexOptions.IgnoreCase),// Opera Mobi/Tablet
                    new Regex(@"(opera).+version\/([\w\.]+)",RegexOptions.IgnoreCase),// Opera > 9.80
                    new Regex(@"(opera)[\/\s]+([\w\.]+)",RegexOptions.IgnoreCase)// Opera < 9.80
                },
                Action = BrowserNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(opios)[\/\s]+([\w\.]+)",RegexOptions.IgnoreCase)// Opera mini on iphone >= 8.0
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Opera Mini";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"\s(opr)\/([\w\.]+)",RegexOptions.IgnoreCase)// Opera Webkit
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Opera";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(kindle)\/([\w\.]+)",RegexOptions.IgnoreCase),// Kindle
                    new Regex(@"(lunascape|maxthon|netfront|jasmine|blazer)[\/\s]?([\w\.]+)*",RegexOptions.IgnoreCase),// Lunascape/Maxthon/Netfront/Jasmine/Blazer
                    
                    new Regex(@"(avant\s|iemobile|slim|baidu)(?:browser)?[\/\s]?([\w\.]*)",RegexOptions.IgnoreCase), // Avant/IEMobile/SlimBrowser/Baidu
                    new Regex(@"(?:ms|\()(ie)\s([\w\.]+)",RegexOptions.IgnoreCase),// Internet Explorer
                    
                    new Regex(@"(rekonq)\/([\w\.]+)*",RegexOptions.IgnoreCase),// Rekonq
                    new Regex(@"(chromium|flock|rockmelt|midori|epiphany|silk|skyfire|ovibrowser|bolt|iron|vivaldi|iridium|phantomjs)\/([\w\.-]+)",RegexOptions.IgnoreCase), // Chromium/Flock/RockMelt/Midori/Epiphany/Silk/Skyfire/Bolt/Iron/Iridium/PhantomJS
                },
                Action = BrowserNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(trident).+rv[:\s]([\w\.]+).+like\sgecko",RegexOptions.IgnoreCase)// IE11
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    current.BrowserName = "IE";
                    current.BrowserVersion = "11";
                }
            },

            new Expression
            {
                Regexes = new List<Regex>{
                    new Regex(@"(edge)\/((\d+)?[\w\.]+)",RegexOptions.IgnoreCase),// Microsoft Edge
                },
                Action = BrowserNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(yabrowser)\/([\w\.]+)",RegexOptions.IgnoreCase)// Yandex
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Yandex";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(comodo_dragon)\/([\w\.]+)",RegexOptions.IgnoreCase)// Comodo Dragon
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = nameAndVersion[0].Replace('_',' ');
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(micromessenger)\/([\w\.]+)",RegexOptions.IgnoreCase)// WeChat
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "WeChat";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"xiaomi\/miuibrowser\/([\w\.]+)",RegexOptions.IgnoreCase)// MIUI Browser
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "MIUI Browser";
                    current.BrowserVersion = nameAndVersion[0];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"\swv\).+(chrome)\/([\w\.]+)",RegexOptions.IgnoreCase)// Chrome WebView
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = new Regex("(.+)").Replace(nameAndVersion[0],"$1 WebView");
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"android.+samsungbrowser\/([\w\.]+)",RegexOptions.IgnoreCase),
                    new Regex(@"android.+version\/([\w\.]+)\s+(?:mobile\s?safari|safari)*",RegexOptions.IgnoreCase)// Android Browser
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Android Browser";
                    current.BrowserVersion = nameAndVersion[0];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(chrome|omniweb|arora|[tizenoka]{5}\s?browser)\/v?([\w\.]+)",RegexOptions.IgnoreCase),// Chrome/OmniWeb/Arora/Tizen/Nokia
                    new Regex(@"(qqbrowser)[\/\s]?([\w\.]+)",RegexOptions.IgnoreCase)// QQBrowser
                },
                Action = BrowserNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(uc\s?browser)[\/\s]?([\w\.]+)",RegexOptions.IgnoreCase),
                    new Regex(@"ucweb.+(ucbrowser)[\/\s]?([\w\.]+)",RegexOptions.IgnoreCase),
                    new Regex(@"juc.+(ucweb)[\/\s]?([\w\.]+)",RegexOptions.IgnoreCase),// UCBrowser
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Android Browser";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(dolfin)\/([\w\.]+)",RegexOptions.IgnoreCase)// Dolphin
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Dolphin";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"((?:android.+)crmo|crios)\/([\w\.]+)",RegexOptions.IgnoreCase)// Chrome for Android/iOS
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Chrome";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@";fbav\/([\w\.]+);",RegexOptions.IgnoreCase)// Facebook App for iOS
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Facebook";
                    current.BrowserVersion = nameAndVersion[0];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"fxios\/([\w\.-]+)",RegexOptions.IgnoreCase)// Firefox for iOS
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Firefox";
                    current.BrowserVersion = nameAndVersion[0];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"version\/([\w\.]+).+?mobile\/\w+\s(safari)",RegexOptions.IgnoreCase)// Mobile Safari
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Mobile Safari";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"version\/([\w\.]+).+?(mobile\s?safari|safari)",RegexOptions.IgnoreCase)// Safari & Safari Mobile
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = nameAndVersion[1];
                    current.BrowserVersion = nameAndVersion[0];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"webkit.+?(mobile\s?safari|safari)(\/[\w\.]+)",RegexOptions.IgnoreCase)// Safari < 3.0
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = nameAndVersion[0];

                    var version = nameAndVersion[1];

                    current.BrowserVersion = _browserVersionMap.Keys.Any(m=>m==version)? _browserVersionMap[version]:version;
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(konqueror)\/([\w\.]+)",RegexOptions.IgnoreCase),// Konqueror
                    new Regex(@"(webkit|khtml)\/([\w\.]+)",RegexOptions.IgnoreCase)
                },
                Action = BrowserNameVersionAction
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(navigator|netscape)\/([\w\.-]+)",RegexOptions.IgnoreCase)// Netscape
                },
                Action = (Match match,System.Object obj) =>
                {
                    ClientOsBrowser current = obj as ClientOsBrowser;

                    var nameAndVersion = match.Value.Split('/');

                    current.BrowserName = "Netscape";
                    current.BrowserVersion = nameAndVersion[1];
                }
            },

            new Expression
            {
                Regexes = new List<Regex>
                {
                    new Regex(@"(swiftfox)",RegexOptions.IgnoreCase),// Swiftfox
                    new Regex(@"(icedragon|iceweasel|camino|chimera|fennec|maemo\sbrowser|minimo|conkeror)[\/\s]?([\w\.\+]+)",RegexOptions.IgnoreCase),// IceDragon/Iceweasel/Camino/Chimera/Fennec/Maemo/Minimo/Conkeror
                    new Regex(@"(firefox|seamonkey|k-meleon|icecat|iceape|firebird|phoenix)\/([\w\.-]+)",RegexOptions.IgnoreCase),// Firefox/SeaMonkey/K-Meleon/IceCat/IceApe/Firebird/Phoenix
                    new Regex(@"(mozilla)\/([\w\.]+).+rv\:.+gecko\/\d+",RegexOptions.IgnoreCase),// Mozilla
                    new Regex(@"(polaris|lynx|dillo|icab|doris|amaya|w3m|netsurf|sleipnir)[\/\s]?([\w\.]+)",RegexOptions.IgnoreCase),// Polaris/Lynx/Dillo/iCab/Doris/Amaya/w3m/NetSurf/Sleipnir
                    new Regex(@"(links)\s\(([\w\.]+)",RegexOptions.IgnoreCase),// Links
                    new Regex(@"(gobrowser)\/?([\w\.]+)*",RegexOptions.IgnoreCase),// GoBrowser
                    new Regex(@"(ice\s?browser)\/v?([\w\._]+)",RegexOptions.IgnoreCase),// ICE Browser
                    new Regex(@"(mosaic)[\/\s]([\w\.]+)",RegexOptions.IgnoreCase)// Mosaic
                },
                Action = BrowserNameVersionAction
            },
        };
    }
}
