Imports System
Imports System.Threading
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.Extensions
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.Interactions
Imports WebDriverManager
Imports WebDriverManager.DriverConfigs
Imports WebDriverManager.DriverConfigs.Impl
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.NetworkInformation
Imports System.IO.File
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks.Task
Imports WinFormsApp1.MyLogging
Imports WebDriverManager.Helpers
Imports System.Reflection.Metadata
Imports System.Collections.Specialized
Imports System.Runtime.Intrinsics
Imports System.DirectoryServices.ActiveDirectory
Imports AngleSharp.Dom.Events
Imports Microsoft.VisualBasic.ApplicationServices

Public Class MyWebDriver
    Public chromeDriver As IWebDriver
    Public act As Actions
    'Dim webDriverWait As WebDriverWait

    Dim rnd_num As New Random()

    'Public css_selector_config_obj As Newtonsoft.Json.Linq.JObject
    'Public m_css_selector_config_obj As Newtonsoft.Json.Linq.JObject

    Public used_browser As String = ""
    Public used_dev_model As String = "PC"
    Public used_chrome_profile As String = ""
    Public running_chrome_profile As String = ""

    Public running_chrome_profile_path As String = ""
    'Dim webDriverWait As WebDriverWait

    Public langConverter As Newtonsoft.Json.Linq.JObject

    Public used_lang = "zh-TW"

    Public headless_mode = False

    Public Profile_Queue() As String

    Public IsUploadImage = False


    Dim css_selector_config As String = System.IO.File.ReadAllText("css_selector_config.json")
    Dim css_selector_config_obj = JsonConvert.DeserializeObject(css_selector_config)

    Dim m_css_selector_config As String = System.IO.File.ReadAllText("m_css_selector_config.json")
    Dim m_css_selector_config_obj = JsonConvert.DeserializeObject(m_css_selector_config)





    Public Function Open_Browser_Task(browser As String, devicetype As String, profile As String)
        Return Task.Run(Function() Open_Browser(browser, devicetype, profile))
    End Function

    Public Function Open_Browser(browser As String, devicetype As String, profile As String)
        'Debug.WriteLine("profile : " + profile)
        'Debug.WriteLine("browser : " + browser)

        If My.Computer.FileSystem.FileExists(profile + "\ProfileInfo.txt") Then
            Dim JsonString As String = System.IO.File.ReadAllText(profile + "\ProfileInfo.txt")
            Dim Profile_JsonObject As Newtonsoft.Json.Linq.JObject
            Profile_JsonObject = JsonConvert.DeserializeObject(JsonString)
            Dim lang = Profile_JsonObject.Item("LanguagePack").ToString()

            If lang <> "" Then
                used_lang = lang
            End If

            'Debug.WriteLine("lang : " + lang)

        End If

        Select Case used_lang
            Case "zh-TW"
                langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-TW.json"))
            Case "zh-HK"
                langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-HK.json"))
            Case "zh-CN"
                langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-CN.json"))
            Case "en-US"
                langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\en-US.json"))
        End Select

        'Debug.WriteLine(langConverter.Item("Create_Post"))

        If browser = "Chrome" Then
            Try
                '########### ChromeDriver Manger ##################
                Dim driverManager = New DriverManager()
                'driverManager.SetUpDriver(New ChromeConfig(), "115.0.5790.99") 'Use specify version.
                'driverManager.SetUpDriver(New ChromeConfig()) 'Automatic download the lastest version and use it.
                driverManager.SetUpDriver(New ChromeConfig(), VersionResolveStrategy.MatchingBrowser) 'automatically download a chromedriver.exe matching the version of the browser
                '#################################################

                Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
                serv.HideCommandPromptWindow = True 'hide cmd
                Dim options = New Chrome.ChromeOptions()

                Dim chrome_path = "C:\Program Files\Google\Chrome\Application\chrome.exe"
                If System.IO.File.Exists("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe") Then
                    chrome_path = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                End If
                Dim processStartInfo As New ProcessStartInfo(chrome_path) ' for remote chrome

                Dim chrome_argv = "--remote-debugging-port=9222 --disable-popup-blocking --disable-notifications --disable-extensions "
                used_dev_model = devicetype
                used_browser = "Chrome"
                If devicetype <> "PC" Then
                    Select Case devicetype
                        Case "iPhone"
                            Debug.WriteLine("iPhone")
                            chrome_argv += "--user-agent=""Mozilla/5.0 (iPhone; CPU iPhone OS 14_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Mobile/15E148 Safari/604.1"" "
                        Case "Pixel"
                            Debug.WriteLine("Pixel")
                        Case "iPad Mini"
                            Debug.WriteLine("iPad Mini")
                        Case "Surface Pro"
                            Debug.WriteLine("Surface Pro")
                        Case "Nest Hub"
                            Debug.WriteLine("Nest Hub")
                    End Select
                    'options.EnableMobileEmulation(used_dev_model)
                    'Debug.WriteLine(devicetype)

                End If


                If profile <> "" Then
                    options.AddArguments("--user-data-dir=" + profile)
                    running_chrome_profile_path = profile
                    used_chrome_profile = profile.Split("\")(UBound(profile.Split("\")))
                    running_chrome_profile = used_chrome_profile
                    processStartInfo.Arguments = chrome_argv + " --user-data-dir=" + profile
                    'FormInit.Render_profile_CheckedListBox()
                Else
                    processStartInfo.Arguments = chrome_argv
                End If
                options.AddArguments("--disable-notifications", "--disable-popup-blocking", "--disable-blink-features", "--disable-blink-features=AutomationControlled")
                'options.AddArguments("remote-debugging-port=9222", "--disable-notifications", "--disable-popup-blocking", "--disable-blink-features", "--disable-blink-features=AutomationControlled")
                'options.AddExcludedArgument("enable-automation")

                options.DebuggerAddress = "localhost:9222" ' for remote debug browser

                'Minimize windows util headless mode work fine

                If headless_mode Then
                    Debug.WriteLine("Headless mode")
                    options.AddArguments("--headless", "--disable-gpu")
                    'chromeDriver.Manage().Window().Minimize()
                End If

                'Dim auto_adjust_window = False
                If False Then ' True for turn on
                    Dim workarea_Hight As Integer
                    Dim workerarea_width As Integer
                    workarea_Hight = Screen.PrimaryScreen.WorkingArea.Width
                    workerarea_width = Screen.PrimaryScreen.WorkingArea.Height
                    options.AddArgument(" --window-size=" & workarea_Hight / 2 & "," & workerarea_width)
                    options.AddArgument(" --window-position=" & workarea_Hight / 2 & "," & "0")
                End If


                'chromeDriver = New ChromeDriver(serv, options)


                '######################### remote debug address test ###############################

                Dim chromeProcess As Process = Process.Start(processStartInfo)
                chromeProcess.WaitForInputIdle()
                'chromeProcess.WaitForExit()
                chromeDriver = New ChromeDriver(serv, options)

                Dim mainWindowHandle As String = chromeDriver.WindowHandles.First()

                chromeDriver.SwitchTo().Window(mainWindowHandle)


                'chromeDriver.Navigate.GoToUrl("https://www.facebook.com/")

                '######################### remote debug address test ###############################

                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
                ' Refresh Profile Items


                act = New Actions(chromeDriver)

                Return True
            Catch ex As Exception
                Debug.WriteLine(ex)
                Return False
            End Try

        ElseIf browser = "Firefox" Then
            Form1.Open_Firefox()
        ElseIf browser = "Edge" Then
            Form1.Open_Edge()
        End If

        Return False
    End Function

    Public Function Quit_ChromeDriver()
        Try

            Dim chromeProcesses() As Process = Process.GetProcessesByName("chrome")
            For Each chromeProcess As Process In chromeProcesses
                chromeProcess.CloseMainWindow()
                chromeProcess.Close()
            Next
            chromeDriver.Quit()
            Return True
        Catch ex As Exception
            MsgBox("未偵測到Chrome")
            Return False
        End Try

    End Function

    Public Function Navigate_GoToUrl_Task(url) As Task(Of Boolean)
        Return Task.Run(Function() Navigate_GoToUrl(url))
    End Function

    Public Function Navigate_GoToUrl(url As String) As Boolean
        Try
            chromeDriver.Navigate.GoToUrl(url)
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function

    Public Function Login_fb(fb_email As String, fb_passwd As String)
        Try
            Navigate_GoToUrl("https://www.facebook.com/")
            chromeDriver.FindElement(By.Name("email")).SendKeys(fb_email)
            chromeDriver.FindElement(By.Name("pass")).SendKeys(fb_passwd)
            chromeDriver.FindElement(By.Name("pass")).SendKeys(Keys.Return)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Function Get_current_group_name()
        Try
            Dim group_name = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_name_a"))).GetAttribute("innerHTML")
            Return group_name
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub IsInternetConnected()
        If Not My.Computer.Network.Ping("google.com") Then
            MsgBox("Network is unreachable")
            'Write_err_log("Network is unreachable")
        End If

    End Sub


    Public Function Click_By_Text_Task(type, text) As Task(Of Boolean)


        If type = "aria_label" Then
            Return Task.Run(Function() click_by_aria_label(text))
        ElseIf type = "span-text" Then
            Return Task.Run(Function() click_by_span_text(text))
        End If


    End Function

    Public Function click_by_aria_label(str As String) As Boolean
        Try
            chromeDriver.FindElement(By.CssSelector("div[aria-label$='" + str + "']")).Click()
            'Write_log("Click: " + str)
            Return True
        Catch ex As Exception
            'Write_err_log("Click: " + str)
            IsInternetConnected()
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function


    Public Function click_by_span_text(str As String) As Boolean

        Try
            chromeDriver.FindElement(By.XPath("//span[contains(text(),'" + str + "')]")).Click()
            'Write_log("Click: " + str)
            Return True
        Catch ex As Exception
            'Write_err_log("Click: " + str)
            IsInternetConnected()
            'Debug.WriteLine(ex)
            Return False
        End Try
    End Function


    Public Function Click_By_CssSelector_Task(cssSelectorStr)
        Return Task.Run(Function() Click_By_CssSelector(cssSelectorStr))
    End Function


    Public Function Click_By_CssSelector(cssSelectorStr)
        Try
            chromeDriver.FindElement(By.CssSelector(cssSelectorStr)).Click()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Click_element_by_feature_Task(parm)
        Return Task.Run(Function() Click_element_by_feature(parm))
    End Function

    Public Function Click_element_by_feature(parm As String)
        Select Case parm
            Case "留個言吧"
                Return Click_leave_message()
            Case "發佈"
                Try
                    chromeDriver.FindElement(By.CssSelector("div[aria-label='發佈']")).Click()
                    'Write_log("Click: " + str)
                    Return True
                Catch ex As Exception
                    'Write_err_log("Click: " + str)
                    IsInternetConnected()
                    Debug.WriteLine(ex)
                    Return False
                End Try
            Case "回覆上"
                Return Click_Top_Reply()
            Case "回覆下"
                Return Click_Bottom_Reply()
        End Select

        Return False
    End Function


    Public Function IsElementPresentByCssSelector_Task(parm)
        Return Task.Run(Function() IsElementPresentByCssSelector(parm))
    End Function

    Function IsElementPresentByCssSelector(locatorKey As String) As Boolean

        Try
            chromeDriver.FindElement(By.CssSelector(locatorKey))
            Return True
        Catch ex As Exception
            'Debug.WriteLine(ex)
            Return False
        End Try

    End Function

    Function IsElementPresentByXpath(locatorXpath As String) As Boolean
        Try
            chromeDriver.FindElement(By.XPath(locatorXpath))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function IsElementPresentByClass_Task(parm)
        Return Task.Run(Function() IsElementPresentByClass(parm))
    End Function

    Function IsElementPresentByClass(ClassName As String) As Boolean
        Try
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)
            chromeDriver.FindElement(By.ClassName(ClassName))
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True
        Catch ex As Exception
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return False
        End Try
    End Function

    Public Function Upload_reply_img_Task(img)
        ' Dim myimage = Bitmap.FromFile(img)
        'Clipboard.SetImage(myimage)
        Dim f() As String = {img}
        Dim d As New DataObject(DataFormats.FileDrop, f)
        Clipboard.SetDataObject(d, True)

        Return Task.Run(Function() Upload_reply_img(img))
    End Function

    Public Function Upload_reply_img(img)
        Debug.WriteLine("img : " + img)

        IsUploadImage = True

        Try
            'Dim comment_img_input As Object

            'If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
            'comment_img_input = chromeDriver.FindElements(By.CssSelector("div[aria-label^='回覆']"))
            'Else
            'comment_img_input = chromeDriver.FindElements(By.CssSelector("div[aria-label^='留言'] > p"))
            'End If


            'If headless_mode Then
            'Debug.WriteLine("headless mode")
            'Dim image_input = chromeDriver.FindElement(By.CssSelector("div.x4b6v7d.x1ojsi0c > ul > li:nth-child(3) > input"))
            'image_input.SendKeys(img)
            'Else
            'Debug.WriteLine("Copy " + img)
            act.KeyDown(Keys.Control).SendKeys("v").Perform()
            act.KeyUp(Keys.Control).Perform()
            'comment_img_input(0).SendKeys(Keys.LeftControl + "v")
            'End If

            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False

        End Try

        'old code keep until stable
        'chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1)

    End Function


    Public Function Upload_Story_img_Task(img)
        Return Task.Run(Function() Upload_Story_img(img))
    End Function


    Public Function Upload_Story_img(img)
        Try
            chromeDriver.Navigate.GoToUrl("https://www.facebook.com/stories/create")
            Dim img_input = chromeDriver.FindElement(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.xdt5ytf.x193iq5w.xeuugli.x1r8uery.x1iyjqo2.xs83m0k.x78zum5.x1t2pt76 > div > div > div > div > input"))
            img_input.SendKeys(img)

            If IsElementPresentByCssSelector("div.x5yr21d.x10l6tqk.xtzzx4i.xwa60dl > div > div > img") Then
                Thread.Sleep(1000)
                chromeDriver.FindElement(By.CssSelector("div.x6s0dn4.x1jx94hy.x10h3on.x78zum5.x1q0g3np.xy75621.x1qughib.x1ye3gou.xn6708d > div:nth-child(2) > div")).Click()

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Async Function Reply_Random_Match_TextAndImage(content) As Task(Of Boolean)
        'Debug.WriteLine(content)
        Dim AllConditions() As String = content.Split(";")
        Dim rnd = rnd_num.Next(0, AllConditions.Length)

        'Debug.WriteLine(AllConditions(rnd))

        Dim TextFolder = AllConditions(rnd).Split("%20")(0)
        Dim ImageFolder = AllConditions(rnd).Split("%20")(1)

        Dim Txtfiles() As String = IO.Directory.GetFiles(FormInit.text_folder_path + TextFolder)
        Dim TextFile_ArrayList = New ArrayList()
        For Each file As String In Txtfiles
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".txt" Then
                TextFile_ArrayList.Add(file)
            End If
        Next

        rnd = rnd_num.Next(0, TextFile_ArrayList.Count)

        If Await Send_reply_comment_Task(File.ReadAllText(TextFile_ArrayList(rnd))) = False Then ' send text 
            Return False
        End If


        Dim Imgfiles() As String = IO.Directory.GetFiles(FormInit.image_folder_path + ImageFolder)
        Dim ImgageFile_ArrayList = New ArrayList()
        For Each file As String In Imgfiles
            'Debug.WriteLine(file)
            ImgageFile_ArrayList.Add(file)
        Next

        rnd = rnd_num.Next(0, ImgageFile_ArrayList.Count)
        'Debug.WriteLine("Image : " + ImgageFile_ArrayList(rnd))

        If Await Upload_reply_img_Task(ImgageFile_ArrayList(rnd)) = False Then   'upload img
            Return False
        End If

        Return True

    End Function

    Public Function Submit_reply_comment_Task()
        Return Task.Run(Function() Submit_reply_comment())
    End Function

    Public Function Submit_reply_comment()
        Try

            act.SendKeys(Keys.Enter).Perform()
            Return True
            Dim msgbox_ele As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment

                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='回覆']"))
            Else

                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='留言'] > p"))
            End If
            'Debug.WriteLine("msgbox : ")
            'Debug.WriteLine(msgbox_ele(0))
            msgbox_ele(0).SendKeys(Keys.Enter)
            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Public Function Write_post_send_content_Task(content) As Task(Of Boolean)
        Try
            Clipboard.SetText(content)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try

        Return Task.Run(Function() Write_post_send_content(content))
    End Function

    Public Function Write_post_send_content(content) As Boolean
        Try
            Dim str_patterns = JsonConvert.DeserializeObject(langConverter.Item("Create_Post").ToString())
            'Debug.WriteLine("tetsetse  " + str_patterns(0).ToString)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)
            For Each pattern In str_patterns
                'Debug.WriteLine("try : " + pattern.ToString())
                Try
                    Dim xpath = "//div[contains(@aria-label, '" + pattern.ToString() + "')]"
                    'Debug.WriteLine("pattern : " + pattern.ToString())
                    Dim msgbox_ele = chromeDriver.FindElement(By.XPath(xpath))
                    'msgbox_ele.SendKeys(content)
                    msgbox_ele.SendKeys(Keys.LeftControl + "v")
                    'act.KeyDown(Keys.Control).SendKeys("v").Perform()
                    chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
                    Return True
                Catch ex As Exception
                    Debug.WriteLine(ex)
                End Try
                'If IsElementPresentByXpath(xpath) Then
                'End If
            Next
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return False
        Catch ex As Exception
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return False
        End Try

    End Function


    Public Function Clear_post_content_Task() As Task(Of Boolean)
        Return Task.Run(Function() Clear_post_content())
    End Function

    Public Function Clear_post_content() As Boolean
        Try
            Dim msgbox_ele As Object
            Dim str_patterns = JsonConvert.DeserializeObject(langConverter.Item("Create_Post").ToString())
            'Debug.WriteLine("tetsetse  " + str_patterns(0).ToString)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3)
            For Each pattern In str_patterns
                'Debug.WriteLine("try : " + pattern.ToString())
                Dim xpath = "//div[contains(@aria-label, '" + pattern.ToString() + "')]"
                If IsElementPresentByXpath(xpath) Then
                    'Debug.WriteLine("pattern : " + pattern.ToString())
                    msgbox_ele = chromeDriver.FindElement(By.XPath(xpath))
                    msgbox_ele.SendKeys(Keys.LeftControl + "a")
                    msgbox_ele.SendKeys(Keys.Delete)
                    Return True
                End If
            Next
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Function Tring_to_upload_img_Task(image_path) As Task(Of Boolean)
        Return Task.Run(Function() Tring_to_upload_img(image_path))
    End Function

    Public Function Tring_to_upload_img(img_path_str) As Boolean
        Try
            Dim my_img_files() As String = img_path_str.Split(vbLf)

            For Each substring As String In my_img_files

                If Not File.Exists(substring) Then
                    'Debug.Write("not exist " + substring)
                    Return False
                End If
            Next

            'Return False

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)
            Dim upload_img_input As Object
            If IsElementPresentByCssSelector("div.x6s0dn4.x1jx94hy.x1n2xptk.xkbpzyx.xdppsyt.x1rr5fae.x1lq5wgf.xgqcy7u.x30kzoy.x9jhf4c.xev17xk.x9f619.x78zum5.x1qughib.xktsk01.x1d52u69.x1y1aw1k.x1sxyh0.xwib8y2.xurb0ha > div.x78zum5 > div:nth-child(1) > input") Then
                upload_img_input = chromeDriver.FindElement(By.CssSelector("div.x6s0dn4.x1jx94hy.x1n2xptk.xkbpzyx.xdppsyt.x1rr5fae.x1lq5wgf.xgqcy7u.x30kzoy.x9jhf4c.xev17xk.x9f619.x78zum5.x1qughib.xktsk01.x1d52u69.x1y1aw1k.x1sxyh0.xwib8y2.xurb0ha > div.x78zum5 > div:nth-child(1) > input"))
                upload_img_input.SendKeys(img_path_str)
            ElseIf IsElementPresentByCssSelector("div.x1r8uery.x1iyjqo2.x6ikm8r.x10wlt62.x4uap5 > div:nth-child(1) > form > div > div > div.x4b6v7d.x1ojsi0c.x10e4vud.x1bouk6t > ul > li:nth-child(3) > input") Then
                upload_img_input = chromeDriver.FindElement(By.CssSelector("div.x1r8uery.x1iyjqo2.x6ikm8r.x10wlt62.x4uap5 > div:nth-child(1) > form > div > div > div.x4b6v7d.x1ojsi0c.x10e4vud.x1bouk6t > ul > li:nth-child(3) > input"))
                upload_img_input.SendKeys(img_path_str)
            Else
                click_by_aria_label(langConverter.Item("Photo_Video"))
                upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_1").ToString))
                upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
            End If
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True
        Catch ex As Exception
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return False
        End Try

    End Function


    Public Function Block_User_By_Page_Task()
        Return Task.Run(Function() Block_User_By_Page())
    End Function

    Public Function Block_User_By_Page()

        Dim mytabs = chromeDriver.WindowHandles

        Dim Block_langConverter As Newtonsoft.Json.Linq.JObject
        Dim Block_Used_Lang = "zh-TW" ' default lang

        If Form1.Block_User_Lang_ComboBox.Text <> "" Then
            Block_Used_Lang = Form1.Block_User_Lang_ComboBox.Text
        End If

        Select Case Block_Used_Lang
            Case "zh-TW"
                Block_langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-TW.json"))
            Case "zh-HK"
                Block_langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-HK.json"))
            Case "zh-CN"
                Block_langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\zh-CN.json"))
            Case "en-US"
                Block_langConverter = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("langpacks\en-US.json"))
            Case Else
                MsgBox("LangPack Error")
                Return False

        End Select

        chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5)
        For Each mytab In mytabs
            'Debug.WriteLine("tab:" & mytab)
            chromeDriver.SwitchTo.Window(mytab)
            Thread.Sleep(1000)
            If click_by_aria_label(Block_langConverter.Item("See_Options")) = False Then
                Continue For
            End If
            Thread.Sleep(500)
            click_by_span_text(Block_langConverter.Item("Block"))
            Thread.Sleep(500)
            click_by_span_text(Block_langConverter.Item("Confirm"))

        Next
        chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
        Return True

    End Function

    Public Function Click_leave_message()

        Try

            Dim str_patterns = JsonConvert.DeserializeObject(langConverter.Item("Create_Post").ToString())
            'Debug.WriteLine("tetsetse  " + str_patterns(0).ToString)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)
            For Each pattern In str_patterns
                Debug.WriteLine("try : " + pattern.ToString())
                'If IsElementPresentByXpath("//span[contains(text(),'" + pattern.ToString() + "')]") Then
                'Debug.WriteLine("pattern : " + pattern.ToString())
                Try
                    chromeDriver.FindElement(By.XPath("//span[contains(text(),'" + pattern.ToString() + "')]")).Click()
                    Return True
                Catch ex As Exception

                End Try

                'End If
            Next
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

        Return False

    End Function

    Public Function Click_reply_Task(index)
        Return Task.Run(Function() Click_reply(index))
    End Function

    Public Function Click_reply(ByVal index As Integer)
        Try

            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                Debug.WriteLine("Comment")
                chromeDriver.FindElements(By.CssSelector("div.xq8finb.x16n37ib.x3dsekl.x1uuop16 > div > div:nth-child(2) > div")).ElementAt(index).Click()
            Else
                Debug.WriteLine("Text")
                Try
                    Dim ele = chromeDriver.FindElements(By.CssSelector("div:nth-child(4) > div > div > div:nth-child(1) > div > div.xq8finb.x16n37ib > div > div:nth-child(2) > div"))
                    ele.ElementAt(index).Click()
                    Thread.Sleep(2000)
                    Return True
                Catch ex As Exception
                    Debug.WriteLine(ex)
                    Return False
                End Try

                'Return click_by_aria_label(langConverter.Item("aria-label-Leave-a-comment").ToString())
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function Click_Top_Reply()
        Try
            chromeDriver.FindElements(By.CssSelector("div.x1r8uery.x1iyjqo2.x6ikm8r.x10wlt62.x1pi30zi > ul > li: nth-child(2) > div")).First.Click()
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Private Function Click_Bottom_Reply()
        Try
            Dim myelement = chromeDriver.FindElements(By.CssSelector("div.x1r8uery.x1iyjqo2.x6ikm8r.x10wlt62.x1pi30zi > ul > li:nth-child(2) > div"))
            myelement.Last.Click()
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Public Function Send_reply_comment_Task(content)
        'If Not File.Exists(content) Then
        'Return False
        'End If
        Clipboard.SetText(content)
        Return Task.Run(Function() Send_reply_comment(content))
    End Function

    Public Function Send_reply_comment(content) As Boolean
        'Dim msgbox_ele As Object
        'chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1)
        Try
            'Dim msgbox_ele As Object
            'If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
            '   msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='回覆']"))
            'Else
            'msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='留言'] > p"))
            '   msgbox_ele = chromeDriver.FindElements(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.notranslate"))
            'End If

            Dim msgbox_ele As Object
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3)
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment

                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='回覆']"))
            Else

                act.KeyDown(Keys.Control).SendKeys("v").Perform()
                act.KeyUp(Keys.Control).Perform()
                'act.SendKeys(Keys.LeftControl + "v").Perform()

                'If IsElementPresentByCssSelector("div[aria-label^='留言'] > p > span") Then
                'msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='留言'] > p > span"))
                '
                'Else
                ' msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='留言'] > p"))
                'End If


            End If


            'If headless_mode Then

            'Dim str_arr() As String = content.Split(vbCrLf)
            'For Each line As String In str_arr
            'line = line.Replace(vbCrLf, "").
            'msgbox_ele(0).SendKeys(line)
            'Thread.Sleep(300)
            'msgbox_ele(0).SendKeys(Keys.LeftShift + Keys.Return)
            'Next

            'Else
            'msgbox_ele(0).SendKeys(Keys.LeftControl + "v")
            'End If
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True

        Catch ex As Exception
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function


    Public Function Click_reply_random_emoji_Task(Emoji_str)
        Return Task.Run(Function() Click_reply_random_emoji(Emoji_str))
    End Function

    Public Function Click_reply_random_emoji(Emoji_str)
        If Emoji_str = "" Then
            'Debug.WriteLine("Empty")
            Return False
        End If

        Dim Emoji_arr() As String = Split(Trim(Emoji_str))
        Dim Used_emoji = ""

        If Emoji_arr.Length <> 0 Then
            Dim rnd_num As New Random()
            Dim random = rnd_num.Next(0, Emoji_arr.Length)
            Used_emoji = Emoji_arr(random)
        End If

        Try
            Dim searchBtn As IWebElement = chromeDriver.FindElement(By.XPath("//span[text()='" + langConverter.Item("Like").ToString() + "']"))
            Dim actionProvider As Actions = New Actions(chromeDriver)
            actionProvider.ClickAndHold(searchBtn).Build().Perform()
            Thread.Sleep(1000)

            Select Case Used_emoji
                Case "讚好"
                    Return click_by_aria_label(langConverter.Item("Like").ToString())
                Case "愛心"
                    Return click_by_aria_label(langConverter.Item("Love").ToString())
                Case "加油"
                    Return click_by_aria_label(langConverter.Item("Care").ToString())
                Case "生氣"
                    Return click_by_aria_label(langConverter.Item("Angry").ToString())
                Case "驚訝"
                    Return click_by_aria_label(langConverter.Item("Wow").ToString())
                Case "難過"
                    Return click_by_aria_label(langConverter.Item("Sad").ToString())
                Case "哈哈"
                    Return click_by_aria_label(langConverter.Item("Haha").ToString())

            End Select

        Catch ex As Exception
            Return False
        End Try

        Return False

    End Function


    Public Function Messager_Contact_Task(img_path_str, message)
        Return Task.Run(Function() Messager_Contact(img_path_str, message))
    End Function


    Function Messager_Contact(img_path_str As String, text_files_path As String)

        Try
            'Navigate_GoToUrl("https://www.facebook.com/messages/t/" + Room_Id)


            If img_path_str <> "" Then
                Dim my_img_files() As String = img_path_str.Split(vbLf)

                For Each substring As String In my_img_files
                    Debug.WriteLine(substring)
                    If Not File.Exists(substring) Then
                        Debug.Write("not exist " + substring)
                        Return False
                    End If
                Next

                Dim files_imput = chromeDriver.FindElement(By.CssSelector("div.x6s0dn4.x1ey2m1c.x78zum5.xl56j7k.x10l6tqk.x1vjfegm.x12nagc.xw3qccf.x3oybdh.x1g2r6go.x11xpdln.x1th4bbo > input"))
                files_imput.SendKeys(img_path_str)

            End If


            If text_files_path <> "" Then

                Dim TextFiles = text_files_path.Trim(";"c).Split(";")
                Dim rnd = rnd_num.Next(0, TextFiles.Length)

                Dim msg = File.ReadAllText(text_folder_path + TextFiles(rnd))

                'Return False

                Dim Text_input = chromeDriver.FindElement(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.x1iyjqo2.x1gh3ibb.xisnujt.xeuugli.x1odjw0f.notranslate > p"))

                Dim str_arr() As String = msg.Split(vbLf)
                For Each line As String In str_arr
                    line = line.Replace(vbCr, "").Replace(vbLf, "")
                    Text_input.SendKeys(line)
                    Thread.Sleep(100)
                    Text_input.SendKeys(Keys.LeftShift + Keys.Return)
                Next

            End If


            'Text_input.SendKeys(Keys.Return)
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function

    Public Function Messager_Submit_Content_Task()
        Return Task.Run(Function() Messager_Submit_Content())
    End Function


    Public Function Messager_Submit_Content()
        Try
            Dim Text_input = chromeDriver.FindElement(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.x1iyjqo2.x1gh3ibb.xisnujt.xeuugli.x1odjw0f.notranslate"))
            Text_input.SendKeys(Keys.Return)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Search_Keyword_Task(engine As String, keyword As String)
        Return Task.Run(Function() Search_Keyword(engine, keyword))
    End Function

    Public Function Search_Keyword(engine As String, keyword As String)
        Try
            Select Case engine
                Case "Facebook"
                    'chromeDriver.Navigate.GoToUrl("https://www.facebook.com/search/top/?q=" + keyword)
                    Navigate_GoToUrl("https://www.facebook.com/")

                    If IsElementPresentByCssSelector("div.x9f619.x78zum5.x1s65kcs.xixxii4.x13vifvy.xhtitgo.xds687c.x90ctcv.x12dzrxb.xiimyba.xqmrbw9.x1h737yt") Then
                        Dim search_icon = chromeDriver.FindElement(By.CssSelector("div.x9f619.x78zum5.x1s65kcs.xixxii4.x13vifvy.xhtitgo.xds687c.x90ctcv.x12dzrxb.xiimyba.xqmrbw9.x1h737yt"))
                        act.MoveToElement(search_icon).Perform()

                    End If

                    Dim search_input_selector_str_1 = "div.x5yr21d.x1n2onr6.xh8yej3.x1t2pt76.x1plvlek.xryxfnj > div > div > div > div > div > div > label > input"
                    Dim search_input_selector_str_2 = "div.x9f619.x1s65kcs.x16xn7b0.xixxii4.x17qophe.x13vifvy.xj35x94.xhtitgo.xmy5rp > div > div > div > div > div > label > input"
                    Dim Search_input As Object
                    If IsElementPresentByCssSelector(search_input_selector_str_1) Then
                        Search_input = chromeDriver.FindElement(By.CssSelector(search_input_selector_str_1))

                    ElseIf IsElementPresentByCssSelector(search_input_selector_str_2) Then
                        Search_input = chromeDriver.FindElement(By.CssSelector(search_input_selector_str_2))
                    Else
                        Return False
                    End If

                    'chromeDriver.FindElement(By.CssSelector("input[aria-label$='搜尋 Facebook']")).Click()
                    'Dim Search_input = chromeDriver.FindElement(By.CssSelector("input[aria-label$='搜尋 Facebook']"))
                    Search_input.Clear()
                    Search_input.SendKeys(keyword)
                    Search_input.SendKeys(Keys.Enter)
                Case "Google"
                    'chromeDriver.Navigate.GoToUrl("https://www.google.com.tw/search?q=" + keyword)
                    Navigate_GoToUrl("https://www.google.com.tw/")
                    Dim search_input = chromeDriver.FindElement(By.CssSelector("div.a4bIc > input"))
                    search_input.SendKeys(keyword)
                    search_input.SendKeys(Keys.Enter)
                Case "Youtube"
                    'chromeDriver.Navigate.GoToUrl("https://www.youtube.com/results?search_query=" + keyword)
                    Navigate_GoToUrl("https://www.youtube.com/")
                    Dim Search_input = chromeDriver.FindElement(By.CssSelector("#search-input > input"))
                    Search_input.Clear()
                    Search_input.SendKeys(keyword)
                    Search_input.SendKeys(Keys.Enter)
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
        Return True

    End Function


    Public Function Click_by_Cursor_Offset(x As String, y As String)

        'js code for getting cursor position in brower
        'document.onclick = Function(e)
        '{
        'var x = e.pageX;
        'var y = e.pageY;
        'Alert("User clicked at position (" + x + "," + y + ")")
        '};

        Try
            act.MoveByOffset(x, y).Build.Perform()
            act.Click.Perform()
            act.MoveByOffset(-x, -y).Build.Perform()
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function SendBrowserKeyAction(pkey As String)
        Dim key_list() As String = pkey.Split("+")

        Dim firstKey = Keys.Enter

        Select Case key_list(0)
            Case "ENTER"
                firstKey = Keys.Enter
            Case "ESC"
                firstKey = Keys.Escape
            Case "CTRL"
                firstKey = Keys.LeftControl
            Case "ALT"
                firstKey = Keys.LeftAlt
            Case "SPACE"
                firstKey = Keys.Space
            Case "J"
                firstKey = "j"
            Case "K"
                firstKey = "k"
        End Select

        Try
            If key_list.Length > 1 Then
                act.KeyDown(firstKey).SendKeys(key_list(1)).KeyUp(firstKey).Perform()
            Else
                act.SendKeys(firstKey).Perform()
            End If

            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function

    Public Shared Async Function Delay_msec(msec As Integer) As Task
        Await Task.Delay(msec)
    End Function


    Public Function Change_Fb_Password_Task()
        Return Task.Run(Function() Change_Fb_Password())
    End Function

    Public Function Change_Fb_Password()
        Try
            Dim password_input = chromeDriver.FindElement(By.XPath("//input[@type='password']"))
            Dim new_password = FormComponentController.Generate_Random_Password(10) 'edit password length you want.
            password_input.SendKeys(new_password)

            Dim jsonObject = New JsonProfileInfo()
            jsonObject.Password = new_password
            Dim jsonString = JsonConvert.SerializeObject(jsonObject)
            Dim myfile As System.IO.StreamWriter
            myfile = My.Computer.FileSystem.OpenTextFileWriter(running_chrome_profile_path + "\ProfileInfo.txt", False) 'True : append   'False : overwrite
            myfile.WriteLine(jsonString)
            myfile.Close()

            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function



    Public Function Insert_Post_Background_Task()
        Return Task.Run(Function() Insert_Post_Background())
    End Function

    Public Function Insert_Post_Background()
        Try
            chromeDriver.FindElement(By.CssSelector("div.x1ey2m1c.x9f619.xn6708d.x1swvt13.x10l6tqk.x17qophe.x84fkku > div > div")).Click()
            Dim background_colors = chromeDriver.FindElements(By.CssSelector("div > div.x6ikm8r.x10wlt62.xurb0ha.x1sxyh0.xh8yej3 > div > div"))
            Dim rnd = rnd_num.Next(1, background_colors.Count)
            Thread.Sleep(2000)
            Debug.WriteLine("div.x6ikm8r.x10wlt62.xurb0ha.x1sxyh0.xh8yej3 > div > div:nth-child(" + CStr(rnd) + ") > div")
            chromeDriver.FindElement(By.CssSelector("div.x6ikm8r.x10wlt62.xurb0ha.x1sxyh0.xh8yej3 > div > div:nth-child(" + CStr(rnd) + ") > div")).Click()
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function


    Public Function Upload_Product_Profile_To_Group_Task(content)
        Try
            Dim Product_Profile_Path = FormInit.product_dir_path + content + "\ProductProfile.json"
            If File.Exists(Product_Profile_Path) Then
                chromeDriver.FindElement(By.CssSelector("#MRoot > div > div.async_composer > div.structuredPublisher.feedRevampPadding._3b9 > table > tbody > tr > td:nth-child(1) > button")).Click()
                Dim jsonString = File.ReadAllText(Product_Profile_Path)
                Dim jsonData As Form1.PruductProfileDataType = System.Text.Json.JsonSerializer.Deserialize(Of Form1.PruductProfileDataType)(jsonString)
                Clipboard.SetText(jsonData.ProductName)
                chromeDriver.FindElement(By.CssSelector("input[name$='composer_attachment_sell_title']")).SendKeys(Keys.LeftControl + "V")
                Clipboard.SetText(jsonData.ProdcutDescription)
                Return Task.Run(Function() Upload_Product_Profile_To_Group(content, jsonData))
            End If
        Catch ex As Exception
            Return False
        End Try

        Return False
    End Function


    Public Function Upload_Product_Profile_To_Group(content, jsonData)
        Debug.WriteLine(content)
        Try

            'chromeDriver.FindElement(By.CssSelector("input[name$='composer_attachment_sell_title']")).Click() '.SendKeys(jsonData.ProductName)
            'chromeDriver.ExecuteJavaScript("document.querySelector(""input[name$='composer_attachment_sell_title']"").value = '" & jsonData.ProductName & "'")


            chromeDriver.FindElement(By.CssSelector("input[placeholder$='價格']")).SendKeys(jsonData.ProductPrice)

            chromeDriver.FindElement(By.CssSelector("input[name$='composer_attachment_sell_pickup_note']")).SendKeys(jsonData.ProductLocated)

            chromeDriver.FindElement(By.CssSelector("textarea[data-sigil$='composer-textarea m-textarea-input']")).SendKeys(Keys.LeftControl + "V")
            'chromeDriver.ExecuteJavaScript("document.querySelector(""textarea[data-sigil$='composer-textarea m-textarea-input']"").value = '" & jsonData.ProdcutDescription & "'")

            Debug.WriteLine("TEXT : " + jsonData.ProdcutDescription)

            Dim imgfiles() As String = IO.Directory.GetFiles(FormInit.product_dir_path + content)
            For Each img As String In imgfiles
                'Debug.WriteLine(file)
                If {".jpg", ".jpeg", ".png", ".mp4"}.Contains(Path.GetExtension(img)) Then
                    Debug.WriteLine(img)
                    chromeDriver.FindElement(By.CssSelector("#photo_input")).SendKeys(img)
                End If

            Next



            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function



    Public Function m_Click_Post_Product_Task()
        Return Task.Run(Function() m_Click_Post_Product())
    End Function


    Public Function m_Click_Post_Product()
        Try
            For i = 0 To 10
                Thread.Sleep(2000)
                Try
                    chromeDriver.FindElement(By.CssSelector("button[value$='發佈']")).Click()
                    Return True
                Catch ex As Exception

                End Try

            Next

            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function



    Public Function Upload_Product_Profile_To_Group_IPhone_Task(content)
        Return Task.Run(Function() Upload_Product_Profile_To_Group_IPhone(content))
    End Function


    Public Function Upload_Product_Profile_To_Group_IPhone(content)
        Debug.WriteLine(content)
        Try
            Thread.Sleep(2000)
            chromeDriver.FindElement(By.CssSelector("#screen-root > div > div:nth-child(2) > div:nth-child(6) > div")).Click()

            'Return True

            Dim Product_Profile_Path = FormInit.product_dir_path + content + "\ProductProfile.json"
            If File.Exists(Product_Profile_Path) Then
                Dim jsonString = File.ReadAllText(Product_Profile_Path)

                Dim jsonData As Form1.PruductProfileDataType = System.Text.Json.JsonSerializer.Deserialize(Of Form1.PruductProfileDataType)(jsonString)

                chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[3]/input")).SendKeys(jsonData.ProductName)

                chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[7]/input")).SendKeys(jsonData.ProductPrice)

                chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[10]/div[2]/input")).SendKeys(jsonData.ProductLocated)

                chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[14]/textarea")).SendKeys(jsonData.ProdcutDescription)

                Return True

                Dim imgfiles() As String = IO.Directory.GetFiles(FormInit.product_dir_path + content)
                For Each img As String In imgfiles
                    Debug.WriteLine("##########")
                    If {".jpg", ".jpeg", ".png", ".mp4"}.Contains(Path.GetExtension(img)) Then
                        Debug.WriteLine(img)
                        chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[15]")).Click()
                        Thread.Sleep(1000)
                        chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div[2]/div[2]/div/div/div/div[2]")).Click()

                        Return True
                        'Thread.Sleep(1000)
                        'chromeDriver.FindElement(By.XPath("//*[@id=""app-body""]/input")).SendKeys(img)

                    End If

                Next

            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Click_Sale_Product_Task()
        Return Task.Run(Function() Click_Sale_Product())
    End Function


    Public Function Click_Sale_Product()
        Try

            chromeDriver.FindElement(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.xeuugli.xs83m0k.x1xmf6yo.x1emribx.x1e56ztr.x1i64zmx.xjl7jj.x19h7ccj.xu9j1y6.x7ep2pv > div:nth-child(1) > div > div > div > div > div > div")).Click()
            chromeDriver.FindElement(By.CssSelector("div.xamitd3.x78zum5.x1q0g3np.x1a02dak.x1e56ztr.xw3qccf.x1sliqq.x14vqqas.xh8yej3.x1ni0lre.xlqeb66 > div:nth-child(1) > div > span > div > div")).Click()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function



    Public Function Upload_Product_Profile_Task(content)
        Try
            Dim Product_Profile_Path = FormInit.product_dir_path + content + "\ProductProfile.json"
            If File.Exists(Product_Profile_Path) Then
                Dim jsonString = File.ReadAllText(Product_Profile_Path)
                Dim jsonData As Form1.PruductProfileDataType = System.Text.Json.JsonSerializer.Deserialize(Of Form1.PruductProfileDataType)(jsonString)
                Clipboard.SetText(jsonData.ProductName)
                Return Task.Run(Function() Upload_Product_Profile(content, jsonData))
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function Upload_Product_Profile(content, jsonData)
        Try

            chromeDriver.FindElement(By.CssSelector("div.x9f619.x1ja2u2z.x1k90msu.x6o7n8i.x1qfuztq.x10l6tqk.x17qophe.x13vifvy.x1hc1fzr.x71s49j.xh8yej3 > div > div:nth-child(4) > div:nth-child(3) > div > div > label > div > div > input")).SendKeys(Keys.LeftControl + "V")
            chromeDriver.FindElement(By.CssSelector("div.x9f619.x1ja2u2z.x1k90msu.x6o7n8i.x1qfuztq.x10l6tqk.x17qophe.x13vifvy.x1hc1fzr.x71s49j.xh8yej3 > div > div:nth-child(4) > div:nth-child(4) > div > div > label > div > div > input")).SendKeys(jsonData.ProductPrice)

            chromeDriver.FindElement(By.CssSelector("div.x9f619.x1ja2u2z.x1k90msu.x6o7n8i.x1qfuztq.x10l6tqk.x17qophe.x13vifvy.x1hc1fzr.x71s49j.xh8yej3 > div > div:nth-child(4) > div:nth-child(5) > div > div > div > label")).Click()

            chromeDriver.FindElement(By.CssSelector("div.xu96u03.xm80bdy.x10l6tqk.x13vifvy > div.x1n2onr6 > div > div > div > div > div.x78zum5.xdt5ytf.x1iyjqo2.x1n2onr6 > div > div:nth-child(" & jsonData.ProductCondition + 1 & ")")).Click()
            'chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[10]/div[2]/input")).SendKeys(jsonData.ProductLocated)

            'chromeDriver.FindElement(By.XPath("//*[@id=""screen-root""]/div/div[2]/div[3]/div[14]/textarea")).SendKeys(jsonData.ProdcutDescription)

            'Return True
            Dim img_input = chromeDriver.FindElement(By.CssSelector("div.x9f619.x1ja2u2z.x1k90msu.x6o7n8i.x1qfuztq.x10l6tqk.x17qophe.x13vifvy.x1hc1fzr.x71s49j.xh8yej3 > div > div:nth-child(4) > div.x1gslohp.x1swvt13.x1pi30zi > label:nth-child(2) > input"))

            Dim imgfiles() As String = IO.Directory.GetFiles(FormInit.product_dir_path + content)
            For Each img As String In imgfiles

                If {".jpg", ".jpeg", ".png", ".mp4"}.Contains(Path.GetExtension(img)) Then
                    Debug.WriteLine("#####################################")
                    Debug.WriteLine(img)
                    img_input.SendKeys(img)

                End If

            Next

            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function


    Public Function Upload_Product_Detail_Profile_Task(content)
        Dim Product_Profile_Path = FormInit.product_dir_path + content + "\ProductProfile.json"
        If File.Exists(Product_Profile_Path) Then
            Dim jsonString = File.ReadAllText(Product_Profile_Path)
            Dim jsonData As Form1.PruductProfileDataType = System.Text.Json.JsonSerializer.Deserialize(Of Form1.PruductProfileDataType)(jsonString)
            Clipboard.SetText(jsonData.ProdcutDescription)
            Return Task.Run(Function() Upload_Product_Detail_Profile(content, jsonData))
        End If
        Return False

    End Function

    Public Function Upload_Product_Detail_Profile(content, jsonData)

        Try
            chromeDriver.FindElement(By.CssSelector("div.x1n2onr6.x1ja2u2z.x9f619.x78zum5.xdt5ytf.x193iq5w.x1l7klhg.x1iyjqo2.xs83m0k.x2lwn1j.xyamay9 > div > div > div > div")).Click()
            Dim txt_area = chromeDriver.FindElement(By.CssSelector("div.x1n2onr6.x1ja2u2z.x9f619.x78zum5.xdt5ytf.x193iq5w.x1l7klhg.x1iyjqo2.xs83m0k.x2lwn1j.xyamay9 > div > div > div:nth-child(2) > div > div > label > div > div > textarea"))
            txt_area.Click()
            txt_area.SendKeys(Keys.LeftControl + "V")

            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Public Function Click_Continue_Product_Task()
        Return Task.Run(Function() Click_Continue_Product())
    End Function



    Public Function Click_Continue_Product()
        Try
            click_by_aria_label("繼續")
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function


    Public Function Click_MarketPlace_Product_Task()
        Return Task.Run(Function() Click_MarketPlace_Product())
    End Function

    Public Function Click_MarketPlace_Product()

        Try
            chromeDriver.FindElement(By.CssSelector("div:nth-child(4) > div.x1n2onr6.x1ja2u2z.x9f619.x78zum5.xdt5ytf.x2lah0s.x193iq5w > div > div:nth-child(2) > div > div > div:nth-child(3) > div > div")).Click()

            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function


    Public Function Click_Share_Product_To_Other_Groups_Task(random)
        Return Task.Run(Function() Click_Share_Product_To_Other_Groups(random))
    End Function

    Public Function Click_Share_Product_To_Other_Groups(random As Boolean)

        If random Then
            Debug.WriteLine("Random")
            Return False
        Else
            Try
                Dim group_check_boxes = chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.xdt5ytf.x2lah0s.x193iq5w.xr9ek0c.xjpr12u.xzboxd6.x14l7nz5 > div > div"))

                Dim idx = 0
                For Each checkbox In group_check_boxes
                    If idx > 4 Then
                        checkbox.Click()
                    End If
                    idx += 1
                    If idx > 24 Then
                        Return True
                    End If
                Next

                Return True
            Catch ex As Exception
                Debug.WriteLine(ex)
                Return False
            End Try
        End If


    End Function



    Public Function Click_Post_Product_Task()
        Return Task.Run(Function() Click_Post_Product())
    End Function

    Public Function Click_Post_Product()
        Try
            click_by_aria_label("發佈")
            Return True
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

End Class
