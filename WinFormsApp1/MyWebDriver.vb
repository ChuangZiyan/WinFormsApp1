Imports System
Imports System.Threading
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Edge
Imports OpenQA.Selenium.Firefox
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
    'Dim webDriverWait As WebDriverWait

    Public langConverter As Newtonsoft.Json.Linq.JObject

    Public used_lang = "zh-TW"

    Public Profile_Queue() As String

    Dim css_selector_config As String = System.IO.File.ReadAllText("css_selector_config.json")
    Dim css_selector_config_obj = JsonConvert.DeserializeObject(css_selector_config)

    Dim m_css_selector_config As String = System.IO.File.ReadAllText("m_css_selector_config.json")
    Dim m_css_selector_config_obj = JsonConvert.DeserializeObject(m_css_selector_config)



    Public Function Open_Browser_Task(browser As String, devicetype As String, profile As String)
        Return Task.Run(Function() Open_Browser(browser, devicetype, profile))
    End Function

    Public Function Open_Browser(browser As String, devicetype As String, profile As String)

        If profile = "全部隨機" Then
            Dim allProfileItem = Form1.Profile_CheckedListBox.Items
            Dim rnd = rnd_num.Next(0, allProfileItem.Count)
            profile = curr_path + allProfileItem(rnd)
        Else
            Dim ProfileItem = profile.Split(";")
            Dim rnd = rnd_num.Next(0, ProfileItem.Length)
            profile = ProfileItem(rnd)

        End If

        'Debug.WriteLine("profile : " + profile)

        If My.Computer.FileSystem.FileExists(profile + "\ProfileInfo.txt") Then
            Dim JsonString As String = System.IO.File.ReadAllText(profile + "\ProfileInfo.txt")
            Dim Profile_JsonObject As Newtonsoft.Json.Linq.JObject
            Profile_JsonObject = JsonConvert.DeserializeObject(JsonString)
            Dim lang = Profile_JsonObject.Item("LanguagePack").ToString()
            used_lang = lang
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
                Dim driverManager = New DriverManager()
                'driverManager.SetUpDriver(New ChromeConfig(), "106.0.5249.61") 'Use specify version.
                'driverManager.SetUpDriver(New ChromeConfig()) 'Automatic download the lastest version and use it.
                driverManager.SetUpDriver(New ChromeConfig(), VersionResolveStrategy.MatchingBrowser) 'automatically download a chromedriver.exe matching the version of the browser
                Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
                serv.HideCommandPromptWindow = True 'hide cmd
                Dim options = New Chrome.ChromeOptions()
                If profile <> "" Then
                    options.AddArguments("--user-data-dir=" + profile)
                    used_chrome_profile = profile.Split("\")(UBound(profile.Split("\")))
                    running_chrome_profile = used_chrome_profile
                End If
                options.AddArguments("--disable-notifications", "--disable-popup-blocking")



                options.AddExcludedArgument("enable-automation")

                'Dim auto_adjust_window = False
                If False Then ' True for turn on
                    Dim workarea_Hight As Integer
                    Dim workerarea_width As Integer
                    workarea_Hight = Screen.PrimaryScreen.WorkingArea.Width
                    workerarea_width = Screen.PrimaryScreen.WorkingArea.Height
                    options.AddArgument(" --window-size=" & workarea_Hight / 2 & "," & workerarea_width)
                    options.AddArgument(" --window-position=" & workarea_Hight / 2 & "," & "0")
                End If



                used_dev_model = devicetype
                used_browser = "Chrome"
                If devicetype <> "PC" Then
                    options.EnableMobileEmulation(used_dev_model)
                End If
                chromeDriver = New ChromeDriver(serv, options)
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
                ' Refresh Profile Items


                'Minimize windows util headless mode work fine
                If Form1.Headless_Mode_Checkbox.Checked Then
                    'options.AddArguments("--headless", "--disable-gpu")
                    chromeDriver.Manage().Window().Minimize()
                End If

                act = New Actions(chromeDriver)

                Form1.Profile_CheckedListBox.Items.Clear()
                Render_profile_CheckedListBox()
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
            chromeDriver.Quit()
            Return True
        Catch ex As Exception
            MsgBox("未偵測到Chrome")
            Return False
        End Try

    End Function

    Public Function Navigate_GoToUrl_Task(url)
        Return Task.Run(Function() Navigate_GoToUrl(url))
    End Function

    Public Function Navigate_GoToUrl(url As String)
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

    Public Function Click_element_by_feature_Task(parm)
        Return Task.Run(Function() Click_element_by_feature(parm))
    End Function

    Public Function Click_element_by_feature(parm As String)
        Select Case parm
            Case "留個言吧"
                Return Click_leave_message()
            Case "發佈"
                Return click_by_aria_label("發佈")
            Case "回覆/留言"
                Return Click_reply()
        End Select

        Return False
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

    Function IsElementPresentByCssSelector(locatorKey As String) As Boolean
        Try
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)
            chromeDriver.FindElement(By.CssSelector(locatorKey))
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True
        Catch ex As Exception
            'Debug.WriteLine(ex)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return False
        End Try

    End Function

    Function IsElementPresentByXpath(locatorXpath As String) As Boolean
        Try
            chromeDriver.FindElement(By.XPath(locatorXpath))
            Return True
        Catch ex As Exception
            'Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Public Function IsElementPresentByClass_Task(parm)
        Return Task.Run(Function() IsElementPresentByClass(parm))
    End Function

    Function IsElementPresentByClass(ClassName As String) As Boolean
        Try
            chromeDriver.FindElement(By.ClassName(ClassName))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Upload_reply_img_Task(img)
        Dim myimage = Bitmap.FromFile(img)
        Clipboard.SetImage(myimage)
        Return Task.Run(Function() Upload_reply_img(img))
    End Function

    Public Function Upload_reply_img(img)

        Try
            'Debug.WriteLine("Copy " + img)
            Dim msgbox_ele = chromeDriver.FindElements(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.notranslate"))
            msgbox_ele(0).SendKeys(Keys.LeftControl + "v")
            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

        ' old code keep until stable
        chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1)

        Try
            Dim comment_img_input As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                comment_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("reply_comment_img_input")))
            Else
                comment_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("reply_post_img_input")))
            End If

            comment_img_input.SendKeys(image_folder_path + img)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True
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

        If Upload_reply_img(ImgageFile_ArrayList(rnd)) = False Then   'upload img
            Return False
        End If

        Return True

    End Function

    Public Function Submit_reply_comment_Task()
        Return Task.Run(Function() Submit_reply_comment())
    End Function

    Public Function Submit_reply_comment()
        Try

            Dim msgbox_ele As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment

                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='回覆']"))
            Else

                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div[aria-label^='留言'] > p"))
                'msgbox_ele = chromeDriver.FindElements(By.CssSelector(".xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.notranslate"))
            End If

            msgbox_ele(0).SendKeys(Keys.Enter)
            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Public Function Write_post_send_content_Task(content) As Task(Of Boolean)
        Return Task.Run(Function() Write_post_send_content(content))
    End Function

    Public Function Write_post_send_content(content) As Boolean
        Try
            Dim str_patterns = JsonConvert.DeserializeObject(langConverter.Item("Create_Post").ToString())
            'Debug.WriteLine("tetsetse  " + str_patterns(0).ToString)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3)
            For Each pattern In str_patterns
                'Debug.WriteLine("try : " + pattern.ToString())
                Dim xpath = "//div[contains(@aria-label, '" + pattern.ToString() + "')]"
                If IsElementPresentByXpath(xpath) Then
                    'Debug.WriteLine("pattern : " + pattern.ToString())
                    Dim msgbox_ele = chromeDriver.FindElement(By.XPath(xpath))
                    msgbox_ele.SendKeys(content)
                    chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
                    Return True
                End If
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

        'Dim ele1 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_1").ToString)
        'Dim ele2 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_2").ToString)

        click_by_aria_label(langConverter.Item("Photo_Video"))

        Dim upload_img_input As Object


        Try
            upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_1").ToString))
            upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function

    Public Sub block_user_btn_Click(sender As Object, e As EventArgs)

        Dim mytabs = chromeDriver.WindowHandles

        For Each mytab In mytabs
            'Debug.WriteLine("tab:" & mytab)
            chromeDriver.SwitchTo.Window(mytab)
            Thread.Sleep(1000)
            click_by_aria_label("查看選項")
            Thread.Sleep(500)
            click_by_span_text("封鎖")
            Thread.Sleep(500)
            'submit
            'click_by_aria_label("確認")
        Next

    End Sub

    Public Function Click_leave_message()

        Try

            Dim str_patterns = JsonConvert.DeserializeObject(langConverter.Item("Create_Post").ToString())
            'Debug.WriteLine("tetsetse  " + str_patterns(0).ToString)
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5)
            For Each pattern In str_patterns
                Debug.WriteLine("try : " + pattern.ToString())
                If IsElementPresentByXpath("//span[contains(text(),'" + pattern.ToString() + "')]") Then
                    Debug.WriteLine("pattern : " + pattern.ToString())
                    chromeDriver.FindElement(By.XPath("//span[contains(text(),'" + pattern.ToString() + "')]")).Click()
                    Return True
                End If
            Next
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

        Return False

    End Function


    Public Function Click_reply()
        Try

            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                chromeDriver.FindElements(By.CssSelector("div.xq8finb.x16n37ib.x3dsekl.x1uuop16 > div > div:nth-child(2) > div")).ElementAt(0).Click()
            Else
                Return click_by_aria_label(langConverter.Item("aria-label-Leave-a-comment").ToString())
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Send_reply_comment_Task(content)
        Clipboard.SetText(content)
        Return Task.Run(Function() Send_reply_comment(content))
    End Function

    Public Function Send_reply_comment(content) As Boolean
        'Dim msgbox_ele As Object
        'chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1)
        Try
            Dim msgbox_ele As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='回覆']"))
            Else
                'msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='留言'] > p"))
                msgbox_ele = chromeDriver.FindElements(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.notranslate"))
            End If



            msgbox_ele(0).SendKeys(Keys.LeftControl + "v")


            'Dim str_arr() As String = content.Split(vbLf)
            'For Each line As String In str_arr
            'line = line.Replace(vbCr, "").Replace(vbLf, "")
            'msgbox_ele.SendKeys(line)
            'Thread.Sleep(100)
            'msgbox_ele.SendKeys(Keys.LeftShift + Keys.Return)
            'Next

            'chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
            Return True

        Catch ex As Exception
            'chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
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


    Function Messager_Contact(Room_Id As String, message As String)

        Try
            Navigate_GoToUrl("https://www.facebook.com/messages/t/" + Room_Id)

            Dim Text_input = chromeDriver.FindElement(By.CssSelector("div.xzsf02u.x1a2a7pz.x1n2onr6.x14wi4xw.x1iyjqo2.x1gh3ibb.xisnujt.xeuugli.x1odjw0f.notranslate"))

            Dim str_arr() As String = message.Split(vbLf)
            For Each line As String In str_arr
                line = line.Replace(vbCr, "").Replace(vbLf, "")
                Text_input.SendKeys(line)
                Thread.Sleep(100)
                Text_input.SendKeys(Keys.LeftShift + Keys.Return)
            Next


            'Text_input.SendKeys(Keys.Return)
            Return True
        Catch ex As Exception
            Return False
            Debug.WriteLine(ex)
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

                    If IsElementPresentByCssSelector("div.x5yr21d.x1n2onr6.xh8yej3.x1t2pt76.x1plvlek.xryxfnj > div > div > div > div > div > div > label") Then
                        chromeDriver.FindElement(By.CssSelector("div.x5yr21d.x1n2onr6.xh8yej3.x1t2pt76.x1plvlek.xryxfnj > div > div > div > div > div > div > label")).Click()
                    End If


                    Navigate_GoToUrl("https://www.facebook.com/")
                    chromeDriver.FindElement(By.CssSelector("input[aria-label$='搜尋 Facebook']")).Click()
                    'Dim Search_input = chromeDriver.FindElement(By.CssSelector("div.x6s0dn4.x9f619.x78zum5.xnnlda6 > div > div > label > input"))
                    Dim Search_input = chromeDriver.FindElement(By.CssSelector("input[aria-label$='搜尋 Facebook']"))
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

    Public Shared Async Function Delay_msec(msec As Integer) As Task
        Await Task.Delay(msec)
    End Function





End Class
