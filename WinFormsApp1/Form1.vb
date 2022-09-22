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

Public Class Form1

    ReadOnly fileContents As String = ReadAllText("C:\selenium_file\fb_auth.txt")


    Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs"

    Dim chromeDriver As IWebDriver
    'Dim webDriverWait As WebDriverWait

    Dim cursor_x As Integer = 0
    Dim cursor_y As Integer = 0

    Dim css_selector_config_obj As Newtonsoft.Json.Linq.JObject
    Dim m_css_selector_config_obj As Newtonsoft.Json.Linq.JObject


    Dim used_browser As String = ""
    Dim dev_model As String = "PC"
    Dim chrome_profile As String = ""

    'Dim webDriverWait As WebDriverWait

    Private Sub Open_browser_Button_Click(sender As Object, e As EventArgs) Handles open_browser_Button.Click
        If chrome_RadioButton.Checked = True Then
            Open_Chrome()
        ElseIf firefox_RadioButton.Checked = True Then
            Open_Firefox()
        ElseIf edge_RadioButton.Checked = True Then
            Open_Edge()
        End If
    End Sub


    Private Function Open_Browser(browser As String, devicetype As String, profile As String)
        Debug.WriteLine(browser)
        Debug.WriteLine(devicetype)
        Debug.WriteLine(profile)
        If browser = "Chrome" Then
            Try
                Dim driverManager = New DriverManager()
                driverManager.SetUpDriver(New ChromeConfig())
                Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
                serv.HideCommandPromptWindow = True 'hide cmd
                Dim options = New Chrome.ChromeOptions()
                If profile <> "" Then
                    options.AddArguments("--user-data-dir=" + profile)
                    chrome_profile = profile.Split("\")(UBound(profile.Split("\")))
                End If
                options.AddArguments("--disable-notifications", "--disable-popup-blocking")
                dev_model = devicetype
                If devicetype <> "PC" Then
                    options.EnableMobileEmulation(dev_model)
                End If
                chromeDriver = New ChromeDriver(serv, options)
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
                Return True
            Catch ex As Exception
                Return False
            End Try

        ElseIf browser = "Firefox" Then
            Open_Firefox()
        ElseIf browser = "Edge" Then
            Open_Edge()
        End If

        Return False
    End Function


    Private Sub Open_Chrome()
        used_browser = "Chrome"

        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New ChromeConfig())

        Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
        serv.HideCommandPromptWindow = True 'hide cmd

        Dim options = New Chrome.ChromeOptions()
        Dim profile = profile_path_TextBox.Text
        If profile <> "" Then
            options.AddArguments("--user-data-dir=" + profile)
            chrome_profile = profile.Split("\")(UBound(profile.Split("\")))
        End If
        options.AddArguments("--disable-notifications", "--disable-popup-blocking")
        If pc_RadioButton.Checked = False Then
            If pixel5_RadioButton.Checked Then
                dev_model = "Pixel 5"
            ElseIf i12pro_RadioButton.Checked Then
                dev_model = "iPhone 12 Pro"
            ElseIf ipadair_RadioButton.Checked Then
                dev_model = "iPad Air"
            End If
            options.EnableMobileEmulation(dev_model)
        End If


        chromeDriver = New ChromeDriver(serv, options)
        chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)

    End Sub

    Private Sub Open_Firefox()
        used_browser = "Firefox"
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New FirefoxConfig())
        Dim firefoxDriver = New FirefoxDriver()
    End Sub

    Private Sub Open_Edge()
        used_browser = "Edge"
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New EdgeConfig())
        Dim edgeDrvier = New EdgeDriver()
    End Sub

    Private Sub Insert_open_browser_btn_Click(sender As Object, e As EventArgs) Handles Insert_open_browser_btn.Click
        If chrome_RadioButton.Checked = True Then
            used_browser = "Chrome"
        ElseIf firefox_RadioButton.Checked = True Then
            used_browser = "Firefox"
        ElseIf edge_RadioButton.Checked = True Then
            used_browser = "Edge"
        End If
        Insert_to_script("開啟", "瀏覽器")
    End Sub


    Private Sub Insert_Login_Button_Click(sender As Object, e As EventArgs) Handles Insert_login_Button.Click
        Dim fb_email = fb_account_TextBox.Text
        Dim fb_passwd = fb_password_TextBox.Text
        Insert_to_script("登入", fb_email + ";" + fb_passwd)

    End Sub

    Public Sub navigate_GoToUrl(url As String)
        Try
            chromeDriver.Navigate.GoToUrl(url)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Get_url_btn_Click(sender As Object, e As EventArgs) Handles Get_url_btn.Click
        Try


        Catch ex As Exception

            Debug.WriteLine(e)
        End Try

    End Sub

    Private Sub get_groupname_Button_Click(sender As Object, e As EventArgs) Handles get_groupname_Button.Click
        If chromeDriver.Url.Contains("groups") AndAlso IsElementPresent(css_selector_config_obj.Item("group_name_a")) Then
            group_name_TextBox.Text = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_name_a"))).GetAttribute("innerHTML")
        Else
            Debug.WriteLine("cant get group name")
        End If
    End Sub

    Private Sub Write_post_Click(sender As Object, e As EventArgs) Handles write_a_post_btn.Click

        'Dim postURL As String = "https://www.facebook.com/groups/737807930865755/posts/737820730864475?comment_id=737820784197803"
        'Dim postURL As String = "https://www.facebook.com/ETtoday/posts/pfbid0Z9CFxwaXaUKQLEUtRMU8aqHomsBiygPgLcqzFXnDYoE8eJ9Qu4ZY9yCvK8tAwzbol?comment_id=608850700553438"
        Dim myURL As String = target_url_TextBox.Text
        Dim img_path_str As String = ""
        chromeDriver.Navigate.GoToUrl(myURL)
        'Write_log("Post to " + myURL)
        'get selected img path into string 
        If img_CheckedListBox.CheckedItems.Count <> 0 Then
            For i = 0 To img_CheckedListBox.CheckedItems.Count - 1
                'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)
                Debug.WriteLine(img_CheckedListBox.Items(i).ToString)
                If img_path_str = "" Then
                    img_path_str = img_CheckedListBox.Items(i).ToString
                Else
                    img_path_str = img_path_str & vbLf & img_CheckedListBox.Items(i).ToString
                End If

            Next
        End If

        If myURL.Contains("groups") Then ' If post in group
            chromeDriver.Navigate.GoToUrl(myURL)

            If click_by_span_text("留個言吧……") = False Then
                Exit Sub
            End If
            If img_path_str <> "" Then
                Tring_to_upload_img(img_path_str)
            End If


            Try
                Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='留個言吧......']"))
                msgbox_ele.SendKeys(content_RichTextBox.Text)
                'Write_log("sendkey to div[aria-label$='留個言吧......']")
            Catch ex As Exception
                'Write_err_log("sendkey to div[aria-label$='留個言吧......']")
                Exit Sub
            End Try

            'chromeDriver.FindElement(By.CssSelector("div[aria-label$='留個言吧......'] > div > div > div")).SendKeys(content_RichTextBox.Text)

            'Dim msgBox = chromeDriver.FindElement(By.CssSelector("._1mf._1mj"))


            'chromeDriver.ExecuteJavaScript(js_code)
            '### submit post ###
            'click_by_span_text("發佈")


        Else ' 
            'Dim btn_eles = chromeDriver.FindElements(By.CssSelector("div.oajrlxb2.g5ia77u1.mtkw9kbi.tlpljxtp.qensuy8j.ppp5ayq2.goun2846.ccm00jje.s44p3ltw.mk2mc5f4.rt8b4zig.n8ej3o3l.agehan2d.sk4xxmp2.rq0escxv.nhd2j8a9.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.l9j0dhe7.i1ao9s8h.esuyzwwr.f1sip0of.du4w35lb.btwxx1t3.abiwlrkh.p8dawk7l.lzcic4wl.bp9cbjyn.ue3kfks5.pw54ja7n.uo3d90p7.l82x9zwi.j83agx80.rj1gh0hx.buofh1pr.g5gj957u.taijpn5t.idt9hxom.cxgpxx05.dflh9lhu.sj5x9vvc.scb9dxdr"))
            'btn_eles.ElementAt(1).Click()
            Dim fail_over = False
            Dim msgbox_ele As Object

            If click_by_span_text("相片／影片") = False Then
                Exit Sub
            End If

            Try
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='在想些什麼？']"))
                msgbox_ele.SendKeys(content_RichTextBox.Text)
                'Write_log("sendkey to div[aria-label$='在想些什麼？']")
            Catch ex As Exception
                'Write_err_log("sendkey to div[aria-label$='在想些什麼？']")
                Exit Sub
            End Try

            Dim img_upload_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("homepage_post_img_input")))

            'Dim img_upload_input = chromeDriver.FindElement(By.CssSelector("div.rq0escxv.l9j0dhe7.du4w35lb.j83agx80.cbu4d94t.pfnyh3mw.d2edcug0.dflh9lhu.scb9dxdr.aahdfvyu.tvmbv18p.gbw9n0fl.fneq0qzw > input"))
            'imgupload_ele.SendKeys("C:\Users\Yan\Desktop\testimg.png")
            'img_upload_input.SendKeys("C:\Users\Yan\Desktop\testimg.png")

            'Debug.WriteLine(img_path_str)
            If img_path_str <> "" Then
                img_upload_input.SendKeys(img_path_str)
            End If


            '### submit post ###
            'chromeDriver.FindElement(By.CssSelector("div.k4urcfbm.discj3wi.dati1w0a.hv4rvrfc.i1fnvgqd.j83agx80.rq0escxv.bp9cbjyn > input")).Click()

        End If



    End Sub

    Private Sub Tring_to_upload_img(img_path_str)


        Dim ele1 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_1").ToString)
        Dim ele2 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_2").ToString)

        If ele1 = False AndAlso ele2 = False Then
            If click_by_aria_label("相片／影片") = False Then
                Exit Sub
            End If
        End If

        'Dim upload_img_input = chromeDriver.FindElement(By.CssSelector("div.fwlpnqze.r5g9zsuq.b0eko5f3.q46jt4gp.p9ctufpz.rj0o91l8.sl27f92c.alzwoclg.bdao358l.jgcidaqu.ta68dy8c.kpwa50dg.m0cukt09.h8391g91.qykh3frn.i0v5kuzt.lkznwk7v.gxnvzty1.k0kqjr44.i85zmo3j > div.alzwoclg > div:nth-child(1) > input"))
        'Dim upload_img_input = chromeDriver.FindElement(By.CssSelector("#toolbarLabel + div > div > input"))
        Dim upload_img_input As Object

        If IsElementPresent(css_selector_config_obj.Item("group_post_img_input_1").ToString) Then
            upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_1").ToString))
            upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
        Else
            Try
                upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_2").ToString))
                upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
                'Write_log("upload img file")
            Catch ex As Exception
                'Write_err_log("upload img file")
                Exit Sub
            End Try

        End If


    End Sub


    Private Sub Get_Groups_Click(sender As Object, e As EventArgs)

        chromeDriver.Navigate.GoToUrl("https://www.facebook.com/groups/feed/")
        Thread.Sleep(3000)
        Try 'if there are more groups, load the groups via button clicked
            chromeDriver.FindElement(By.XPath("//span[contains(text(),'查看更多')]")).Click()
            Thread.Sleep(1000)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try

        Dim scroll_x_value = 2000
        Dim pre_counter As Integer = 0


        While True
            scroll_x_value += 1000
            Dim my_counter As Integer = chromeDriver.FindElements(By.CssSelector("div.goun2846.mk2mc5f4.ccm00jje.s44p3ltw.rt8b4zig.sk4xxmp2.n8ej3o3l.agehan2d.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.hpfvmrgz.jb3vyjys.qt6c0cv9.l9j0dhe7.du4w35lb.bp9cbjyn.btwxx1t3.dflh9lhu.scb9dxdr.nnctdnn4 > div.goun2846.mk2mc5f4.ccm00jje.s44p3ltw.rt8b4zig.sk4xxmp2.n8ej3o3l.agehan2d.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.hpfvmrgz.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.du4w35lb.bp9cbjyn.ns4p8fja.btwxx1t3.l9j0dhe7 > div > div > div > div:nth-child(1) > span > span > span")).Count
            If my_counter = pre_counter Then
                Exit While
            End If

            chromeDriver.ExecuteJavaScript("document.getElementsByClassName(""rpm2j7zs k7i0oixp gvuykj2m j83agx80 cbu4d94t ni8dbmo4 du4w35lb q5bimw55 ofs802cu pohlnb88 dkue75c7 mb9wzai9 d8ncny3e buofh1pr g5gj957u tgvbjcpo l56l04vs r57mb794 kh7kg01d eg9m0zos c3g1iek1 l9j0dhe7 k4xni2cv"")[0].scroll(0," & scroll_x_value & ")")
            pre_counter = my_counter
            Thread.Sleep(1000)
        End While


        Dim group_url_classes = chromeDriver.FindElements(By.CssSelector("a.oajrlxb2.gs1a9yip.g5ia77u1.mtkw9kbi.tlpljxtp.qensuy8j.ppp5ayq2.goun2846.ccm00jje.s44p3ltw.mk2mc5f4.rt8b4zig.n8ej3o3l.agehan2d.sk4xxmp2.rq0escxv.nhd2j8a9.mg4g778l.pfnyh3mw.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.hpfvmrgz.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.l9j0dhe7.i1ao9s8h.esuyzwwr.f1sip0of.du4w35lb.btwxx1t3.abiwlrkh.p8dawk7l.lzcic4wl.ue3kfks5.pw54ja7n.uo3d90p7.l82x9zwi.a8c37x1j"))
        Dim group_name_classes = chromeDriver.FindElements(By.CssSelector("div.goun2846.mk2mc5f4.ccm00jje.s44p3ltw.rt8b4zig.sk4xxmp2.n8ej3o3l.agehan2d.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.hpfvmrgz.jb3vyjys.qt6c0cv9.l9j0dhe7.du4w35lb.bp9cbjyn.btwxx1t3.dflh9lhu.scb9dxdr.nnctdnn4 > div.goun2846.mk2mc5f4.ccm00jje.s44p3ltw.rt8b4zig.sk4xxmp2.n8ej3o3l.agehan2d.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.hpfvmrgz.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.du4w35lb.bp9cbjyn.ns4p8fja.btwxx1t3.l9j0dhe7 > div > div > div > div:nth-child(1) > span > span > span"))
        Debug.WriteLine(group_url_classes.Count)
        For i As Integer = 1 To group_url_classes.Count - 1
            'Debug.WriteLine(group_classes.ElementAt(i).GetAttribute("href"))
            'Group_ListView.Items.Add(group_name_classes.ElementAt(i - 1).GetAttribute("innerHTML"), 100)
            'Group_ListView.Items(i - 1).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
        Next

    End Sub

    Private Sub get_groups_from_m_Click(sender As Object, e As EventArgs)

        Dim curr_row As Integer = 0

        '### Find manage groups ###
        chromeDriver.Navigate.GoToUrl("https://m.facebook.com/groups_browse/your_groups/manage/")
        Thread.Sleep(3000)
        Dim pre_counter As Integer = 0

        While True 'Scroll to the bottom

            Dim my_counter As Integer = chromeDriver.FindElements(By.CssSelector(m_css_selector_config_obj.Item("mng_group_name_classes"))).Count
            If my_counter = pre_counter Then
                Exit While
            End If
            chromeDriver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);")
            pre_counter = my_counter

        End While


        'Get manage groups and add to the listview
        Dim mng_group_name_classes = chromeDriver.FindElements(By.CssSelector(m_css_selector_config_obj.Item("mng_group_name_classes")))
        Dim mng_group_url_classes = chromeDriver.FindElements(By.CssSelector(m_css_selector_config_obj.Item("mng_group_url_classes")))
        For i As Integer = 0 To mng_group_name_classes.Count - 1
            'Debug.WriteLine(group_classes.ElementAt(i).GetAttribute("href"))
            'Group_ListView.Items.Add(mng_group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            'Group_ListView.Items(curr_row).SubItems.Add(mng_group_url_classes.ElementAt(i).GetAttribute("href"))
            curr_row += 1
        Next



        '### Find other groups ###
        chromeDriver.Navigate.GoToUrl("https://m.facebook.com/groups_browse/your_groups/")
        Thread.Sleep(3000)
        pre_counter = 0
        While True ' Scroll to the bottom
            Dim my_counter As Integer = chromeDriver.FindElements(By.CssSelector("div._2pip > div > a > div > div._7ymb._3qn7._61-0._2fyi._3qng > div > div._4ik4._4ik5 > div")).Count
            If my_counter = pre_counter Then
                Exit While
            End If
            chromeDriver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);")
            pre_counter = my_counter
            Thread.Sleep(1000)
        End While


        'Get other groups and add to the listview
        Dim group_name_classes = chromeDriver.FindElements(By.CssSelector("div._2pip > div > a > div > div._7ymb._3qn7._61-0._2fyi._3qng > div > div._4ik4._4ik5 > div"))
        Dim group_url_classes = chromeDriver.FindElements(By.CssSelector("div._7om2 > div > div > div:nth-child(3) > div > div._2pip > div > a"))

        'Debug.WriteLine(group_url_classes.Count)
        For i As Integer = 0 To group_url_classes.Count - 1
            'Debug.WriteLine(group_classes.ElementAt(i).GetAttribute("href"))
            'Group_ListView.Items.Add(group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            'Group_ListView.Items(curr_row).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
            curr_row += 1
        Next

    End Sub


    Private Sub Driver_close_Click(sender As Object, e As EventArgs) Handles driver_close_bnt.Click
        'driver_close_bnt.Enabled = False

        Try
            chromeDriver.Close()
            chromeDriver.Quit()
            refresh_url_timer.Enabled = False
        Catch ex As Exception
            MsgBox("chrome A not exist")
        End Try

    End Sub

    Public Sub quit_chrome()
        chromeDriver.Quit()
    End Sub



    Private Sub Reply_comment_bnt_Click(sender As Object, e As EventArgs) Handles reply_comment_bnt.Click

        chromeDriver.Navigate.GoToUrl(target_url_TextBox.Text)
        Thread.Sleep(1000)

        Dim str_arr() As String = content_RichTextBox.Text.Split(vbLf)
        Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='回覆']"))
        For Each line As String In str_arr
            line = line.Replace(vbCr, "").Replace(vbLf, "")
            msgbox_ele.SendKeys(line)
            Thread.Sleep(100)
            msgbox_ele.SendKeys(Keys.LeftShift + Keys.Return)
            Thread.Sleep(100)
        Next

        If img_CheckedListBox.CheckedItems.Count <> 0 Then
            Debug.WriteLine(img_CheckedListBox.Items(0).ToString())
            Dim comment_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("replay_comment_img_input")))
            comment_img_input.SendKeys(img_CheckedListBox.Items(0).ToString())
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Render_eventlog_listview()



        For Each Dir As String In My.Computer.FileSystem.GetDirectories(My.Computer.FileSystem.CurrentDirectory + "\Chrome")
            'Debug.WriteLine("DIR : " + Dir)
            'chromedriver_ListBox.Items.Add(Dir + "\X64\chromedriver.exe")
            For Each driver_dir As String In My.Computer.FileSystem.GetDirectories(Dir)
                'Debug.WriteLine(driver_dir)
                chromedriver_ListBox.Items.Add(driver_dir + "\chromedriver.exe")

            Next

        Next

        Dim css_selector_config As String = System.IO.File.ReadAllText("css_selector_config.json")
        css_selector_config_obj = JsonConvert.DeserializeObject(css_selector_config)


        Dim m_css_selector_config As String = System.IO.File.ReadAllText("m_css_selector_config.json")
        m_css_selector_config_obj = JsonConvert.DeserializeObject(m_css_selector_config)

        render_img_listbox()
        'Form2.Visible = True

    End Sub
    Private Sub IsInternetConnected()
        If Not My.Computer.Network.Ping("google.com") Then
            MsgBox("Network is unreachable")
            'Write_err_log("Network is unreachable")
        End If

    End Sub

    Private Sub render_img_listbox()
        Dim files() As String = IO.Directory.GetFiles("C:\selenium_file\my_img") ' your img folder

        For Each file As String In files
            'Debug.WriteLine(file)
            img_CheckedListBox.Items.Add(file)
        Next


    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles refresh_url_timer.Tick
        Try
            'Debug.WriteLine(chromeDriver.Url)
            If curr_url_TextBox.Text <> chromeDriver.Url Then
                curr_url_TextBox.Text = chromeDriver.Url
            End If

            If chromeDriver.Url.Contains("groups") AndAlso IsElementPresent(css_selector_config_obj.Item("group_name_a")) Then
                group_name_TextBox.Text = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_name_a"))).GetAttribute("innerHTML")
            End If

        Catch ex As System.NullReferenceException
            Debug.WriteLine(ex)
        End Try



    End Sub


    Private Sub cursor_Click(sender As Object, e As EventArgs) Handles cursor_clickl_btn.Click

        'Dim myele = chromeDriver.FindElement(By.CssSelector("._6ltj > a"))
        'Dim login_btn = chromeDriver.FindElement(By.Name("login"))
        'Dim bbb = chromeDriver.FindElement(By.CssSelector("._8esh"))

        'act.MoveByOffset(0, 0).Build().Perform() ' reset point position
        Dim act = New Actions(chromeDriver)
        cursor_x = cursor_x_TextBox.Text
        cursor_y = cursor_y_TextBox.Text
        act.MoveByOffset(cursor_x, cursor_y).Build.Perform()
        act.Click.Perform()
        Debug.WriteLine(cursor_x & "," & cursor_y)
        act.MoveByOffset(-cursor_x, -cursor_y).Build.Perform()

        'Thread.Sleep(3000)
        'act.MoveToElement(login_btn).Perform()
        'Thread.Sleep(2000)
        'aaa.Click()
        'act.MoveToElement(myele).Perform()
        'Thread.Sleep(2000)
        'act.MoveToElement(bbb).Perform()
        'Thread.Sleep(1000)
        'Thread.Sleep(1000)

    End Sub

    Private Sub hide_btn_Click(sender As Object, e As EventArgs) Handles hide_btn.Click
        MsgBox(My.Computer.FileSystem.CurrentDirectory)
        IO.File.SetAttributes(My.Computer.FileSystem.CurrentDirectory + "\Chrome", IO.FileAttributes.Hidden)
    End Sub

    Private Sub show_btn_Click(sender As Object, e As EventArgs) Handles show_btn.Click
        IO.File.SetAttributes(My.Computer.FileSystem.CurrentDirectory + "\Chrome", IO.FileAttributes.System)
    End Sub

    Private Sub crawl_post_btn_Click(sender As Object, e As EventArgs) Handles crawl_post_btn.Click
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New ChromeConfig())


        Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
        serv.HideCommandPromptWindow = True 'hide cmd

        Dim options = New Chrome.ChromeOptions()
        options.AddArguments("--disable-notifications", "--disable-popup-blocking", "--headless")

        chromeDriver = New ChromeDriver(serv, options)
        Thread.Sleep(1000)
        chromeDriver.Navigate.GoToUrl(target_url_TextBox.Text)
        Thread.Sleep(1000)

        Dim content As String = chromeDriver.FindElement(By.CssSelector("._5_jv._58jw > p")).GetAttribute("innerHTML")

        content_RichTextBox.Text = content
        chromeDriver.Quit()

    End Sub

    Private Sub block_user_btn_Click(sender As Object, e As EventArgs) Handles block_user_btn.Click

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

    Private Function click_by_aria_label(str As String) As Boolean
        Try
            chromeDriver.FindElement(By.CssSelector("div[aria-label$='" + str + "']")).Click()
            'Write_log("Click: " + str)
            Return True
        Catch ex As Exception
            'Write_err_log("Click: " + str)
            IsInternetConnected()
            'Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Private Function click_by_span_text(str As String) As Boolean

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

    Function IsElementPresent(locatorKey As String) As Boolean
        Try
            chromeDriver.FindElement(By.CssSelector(locatorKey))
            Return True
        Catch ex As Exception
            'Debug.WriteLine(ex)
            Return False
        End Try
    End Function

    Private Sub clr_post_btn_Click(sender As Object, e As EventArgs) Handles clr_post_btn.Click
        Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='留個言吧......']"))
        msgbox_ele.SendKeys(Keys.LeftControl + "a")
        msgbox_ele.SendKeys(Keys.Delete)
    End Sub

    Private Sub show_log_btn_Click(sender As Object, e As EventArgs) Handles show_log_btn.Click
        Form2.Visible = True
    End Sub



    Private Sub Insert_to_script(action As String, content As String)
        Dim myline = Date.Now.ToString("yyyy/MM/dd") + "," + Date.Now.ToString("HH:mm:ss") + "," + used_browser + "," + dev_model + "," + chrome_profile + "," + curr_url_TextBox.Text + "," + action + "," + content + ","
        EventlogListview_AddNewItem(myline)
    End Sub

    Public Sub Write_log(action As String, content As String)


        Dim curr_url = "N/A"
        Try
            curr_url = chromeDriver.Url.Replace(",", "")
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
        Dim myline = Date.Now.ToString("yyyy/MM/dd") + "," + Date.Now.ToString("HH:mm:ss") + "," + used_browser + "," + dev_model + "," + chrome_profile + "," + curr_url + "," + action + "," + content + ",成功"
        EventlogListview_AddNewItem(myline)
        Log_to_file(myline)
    End Sub

    Public Sub Write_err_log(action As String, content As String)
        Dim curr_url = "N/A"
        Try
            curr_url = chromeDriver.Url.Replace(",", "")
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try

        Dim myline = Date.Now.ToString("yyyy/MM/dd") + "," + Date.Now.ToString("HH:mm:ss") + "," + used_browser + "," + dev_model + "," + chrome_profile + "," + curr_url + "," + action + "," + content + ",失敗"
        EventlogListview_AddNewItem(myline)
        Log_to_file(myline)
    End Sub

    Public Sub EventlogListview_AddNewItem(content)
        Dim curr_row = script_ListView.Items.Count
        script_ListView.Items.Insert(curr_row, curr_row + 1.ToString)

        Dim splittedLine() As String = content.Split(",")
        For Each log In splittedLine
            script_ListView.Items(curr_row).SubItems.Add(log)
        Next
    End Sub

    Public Sub Log_to_file(content As String)
        Dim thisDate As String = Date.Today.ToString("dd-MM-yyyy")
        Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs\" + thisDate
        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If

        Dim filename_counter As Integer = 1

        Dim max_file_line As Integer = 10

        While True 'check file exist or higher than max line
            If Not My.Computer.FileSystem.FileExists(log_path + "\selenium_log." & filename_counter & ".txt") Then
                Exit While
            End If
            Dim lineCount = ReadAllLines(log_path + "\selenium_log." & filename_counter & ".txt").Length
            'Debug.WriteLine(lineCount)
            If lineCount > max_file_line Then
                filename_counter += 1
            Else
                Exit While
            End If
        End While


        Dim log_file As System.IO.StreamWriter
        log_file = My.Computer.FileSystem.OpenTextFileWriter(log_path + "\selenium_log." & filename_counter & ".txt", True)
        log_file.WriteLine(content)
        log_file.Close()


        'write to buffer
        Dim log_file_temp As System.IO.StreamWriter
        log_file_temp = My.Computer.FileSystem.OpenTextFileWriter(My.Computer.FileSystem.CurrentDirectory + "\logs\selenium_log_temp.txt", True)
        log_file_temp.WriteLine(content)
        log_file_temp.Close()
    End Sub


    Private Sub Render_eventlog_listview()
        script_ListView.View = View.Details
        script_ListView.GridLines = True
        script_ListView.FullRowSelect = True
        script_ListView.Columns.Add("編號", 0)
        script_ListView.Columns.Add("日期", 0)
        script_ListView.Columns.Add("時間", 0)
        script_ListView.Columns.Add("瀏覽器", 100)
        script_ListView.Columns.Add("設備", 100)
        script_ListView.Columns.Add("名稱", 100)
        script_ListView.Columns.Add("網址", 100)
        script_ListView.Columns.Add("執行動作", 100)
        script_ListView.Columns.Add("內容", 200)
        script_ListView.Columns.Add("執行結果", 100)


        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If

        If Not My.Computer.FileSystem.FileExists(log_path + "\selenium_log_temp.txt") Then
            Dim log_file_temp As System.IO.StreamWriter
            log_file_temp = My.Computer.FileSystem.OpenTextFileWriter(log_path + "\selenium_log_temp.txt", True)
            log_file_temp.Write("")
            log_file_temp.Close()
        End If

        Update_eventlog_listview()
    End Sub

    Public Sub Update_eventlog_listview()
        script_ListView.Items.Clear()
        Dim log_lines = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Reverse()
        Dim curr_row As Integer = 0
        Dim index As Integer = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Length
        For Each line In log_lines
            Dim splittedLine() As String = line.Split(",")
            script_ListView.Items.Add(index.ToString)
            For Each log In splittedLine
                script_ListView.Items(curr_row).SubItems.Add(log)
            Next
            curr_row += 1
            index -= 1
        Next

    End Sub

    Dim current_state = 0
    Private Sub collapse_btn_Click(sender As Object, e As EventArgs) Handles collapse_btn.Click
        If current_state = 0 Then
            script_ListView.Columns(0).Width = 50
            script_ListView.Columns(1).Width = 100
            script_ListView.Columns(2).Width = 80
            current_state = 1
        Else
            script_ListView.Columns(0).Width = 0
            script_ListView.Columns(1).Width = 0
            script_ListView.Columns(2).Width = 0
            current_state = 0
        End If
    End Sub




    'for the singe action 

    Private Function Login_fb(fb_email As String, fb_passwd As String)
        Try
            navigate_GoToUrl("https://www.facebook.com/")
            chromeDriver.FindElement(By.Name("email")).SendKeys(fb_email)
            chromeDriver.FindElement(By.Name("pass")).SendKeys(fb_passwd)
            chromeDriver.FindElement(By.Name("pass")).SendKeys(Keys.Return)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub Run_script_btn_Click(sender As Object, e As EventArgs) Handles Run_script_btn.Click
        For Each item As ListViewItem In script_ListView.Items
            '3 : browser type
            '4 : device type
            '5 : profile
            '6 : url
            '7 : action ... click sendkey or navigate
            '8 : parameter and content
            '9 : result
            'Debug.WriteLine(item.SubItems.Item(3).Text + "   " + item.SubItems.Item(4).Text)
            Dim brower = item.SubItems.Item(3).Text
            Dim devicetype = item.SubItems.Item(4).Text
            Dim profile = item.SubItems.Item(5).Text
            Dim action = item.SubItems.Item(7).Text
            Dim content = item.SubItems.Item(8).Text
            Select Case action
                Case "開啟"
                    Debug.WriteLine("open brower")
                    If Open_Browser(brower, devicetype, profile) Then
                        item.SubItems.Item(9).Text = "成功"
                    Else
                        item.SubItems.Item(9).Text = "失敗"
                    End If
                Case "登入"
                    Dim auth() As String = content.Split(";")
                    If Login_fb(auth(0), auth(1)) Then
                        item.SubItems.Item(9).Text = "成功"
                    Else
                        item.SubItems.Item(9).Text = "失敗"
                    End If
            End Select
        Next
    End Sub
End Class
