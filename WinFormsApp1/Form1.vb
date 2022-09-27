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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Render_eventlog_listview()


        Dim css_selector_config As String = System.IO.File.ReadAllText("css_selector_config.json")
        css_selector_config_obj = JsonConvert.DeserializeObject(css_selector_config)

        Dim m_css_selector_config As String = System.IO.File.ReadAllText("m_css_selector_config.json")
        m_css_selector_config_obj = JsonConvert.DeserializeObject(m_css_selector_config)

        render_img_listbox()
        'Form2.Visible = True

    End Sub



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
            Debug.WriteLine(profile)
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


    Public Function Navigate_GoToUrl(url As String)
        Try
            chromeDriver.Navigate.GoToUrl(url)
            chromeDriver.Navigate.GoToUrl(url)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub get_groupname_Button_Click(sender As Object, e As EventArgs) Handles get_groupname_Button.Click
        Try
            If chromeDriver.Url.Contains("groups") AndAlso IsElementPresent(css_selector_config_obj.Item("group_name_a")) Then
                group_name_TextBox.Text = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_name_a"))).GetAttribute("innerHTML")
            Else
                Debug.WriteLine("cant get group name")
            End If

        Catch ex As Exception
            MsgBox("未偵測到Chrome")
        End Try

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
            chromeDriver.Quit()
        Catch ex As Exception
            MsgBox("未偵測到Chrome")
        End Try

    End Sub

    Public Function Quit_chromedriver()
        Try
            chromeDriver.Quit()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Sub Reply_comment_bnt_Click(sender As Object, e As EventArgs)

        'chromeDriver.Navigate.GoToUrl(target_url_TextBox.Text)
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


    Dim current_checked As Integer
    Private Sub reply_img_CheckedListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles reply_img_CheckedListBox.SelectedIndexChanged
        reply_img_CheckedListBox.SetItemChecked(current_checked, False)
        For i = 0 To reply_img_CheckedListBox.Items.Count - 1
            'We ask if this item is checked or not
            If reply_img_CheckedListBox.GetItemChecked(i) Then
                current_checked = i
            End If
        Next

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
            reply_img_CheckedListBox.Items.Add(file)
        Next


    End Sub



    Private Sub cursor_Click(sender As Object, e As EventArgs)

        'Dim myele = chromeDriver.FindElement(By.CssSelector("._6ltj > a"))
        'Dim login_btn = chromeDriver.FindElement(By.Name("login"))
        'Dim bbb = chromeDriver.FindElement(By.CssSelector("._8esh"))

        'act.MoveByOffset(0, 0).Build().Perform() ' reset point position
        Dim act = New Actions(chromeDriver)
        'cursor_x = cursor_x_TextBox.Text
        'cursor_y = cursor_y_TextBox.Text
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

    Private Sub hide_btn_Click(sender As Object, e As EventArgs)
        MsgBox(My.Computer.FileSystem.CurrentDirectory)
        IO.File.SetAttributes(My.Computer.FileSystem.CurrentDirectory + "\Chrome", IO.FileAttributes.Hidden)
    End Sub

    Private Sub show_btn_Click(sender As Object, e As EventArgs)
        IO.File.SetAttributes(My.Computer.FileSystem.CurrentDirectory + "\Chrome", IO.FileAttributes.System)
    End Sub

    Private Sub crawl_post_btn_Click(sender As Object, e As EventArgs)
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New ChromeConfig())


        Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
        serv.HideCommandPromptWindow = True 'hide cmd

        Dim options = New Chrome.ChromeOptions()
        options.AddArguments("--disable-notifications", "--disable-popup-blocking", "--headless")

        chromeDriver = New ChromeDriver(serv, options)
        Thread.Sleep(1000)
        'chromeDriver.Navigate.GoToUrl(target_url_TextBox.Text)
        Thread.Sleep(1000)

        Dim content As String = chromeDriver.FindElement(By.CssSelector("._5_jv._58jw > p")).GetAttribute("innerHTML")

        content_RichTextBox.Text = content
        chromeDriver.Quit()

    End Sub

    Private Sub block_user_btn_Click(sender As Object, e As EventArgs)

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




    '####################### Function script for selenium executing ##############################################################

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
            Dim result As Boolean
            Select Case action
                Case "開啟"
                    result = Open_Browser(brower, devicetype, content)
                Case "關閉"
                    result = Quit_chromedriver()
                Case "登入"
                    Dim auth() As String = content.Split(";")
                    result = Login_fb(auth(0), auth(1))
                Case "前往"
                    result = Navigate_GoToUrl(content)
                Case "等待"
                    Try
                        Thread.Sleep(Convert.ToInt64(item.SubItems.Item(8).Text) * 1000)
                        result = True
                    Catch ex As Exception
                        result = False
                    End Try
                Case "點擊"
                    result = Click_element_by_feature(content)
                Case "發送"
                    result = Write_post_send_content(content)
                Case "清空"
                    result = Clear_post_content()
                Case "上載"
                    result = Tring_to_upload_img(content)
                Case "回應:上載"
                    result = Upload_reply_img(content)
                Case "回應:內容"
                    result = Send_reply_comment(content)
                Case "回應:送出"
                    result = Submit_reply_comment()
            End Select

            If result = True Then ' record the result
                item.SubItems.Item(9).Text = "成功"
            ElseIf result = False Then
                item.SubItems.Item(9).Text = "失敗"
            End If

        Next
    End Sub

    Private Function Login_fb(fb_email As String, fb_passwd As String)
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

    Private Function Click_leave_message()

        Try
            Dim myURL = chromeDriver.Url
            If myURL.Contains("groups") Then ' If post in group
                chromeDriver.FindElement(By.XPath("//span[contains(text(),'留個言吧……')]")).Click()
            Else ' personal page
                chromeDriver.FindElement(By.XPath("//span[contains(text(),'在想些什麼？')]")).Click()
            End If
            Return True

        Catch ex As Exception

            Return False
        End Try

        Return False

    End Function


    Private Function Click_reply()
        Try

            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                Debug.WriteLine("reply comment")
                chromeDriver.FindElements(By.CssSelector("div.jg3vgc78.cgu29s5g.lq84ybu9.hf30pyar.r227ecj6 > ul > li:nth-child(2) > div")).ElementAt(0).Click()
            Else
                Debug.WriteLine("left comment")
                Return click_by_aria_label("留言")
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function Send_reply_comment(content)
        Try
            Dim msgbox_ele As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='回覆']"))
            Else
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='留言'] > p"))
            End If

            Dim str_arr() As String = content.Split(vbLf)
            For Each line As String In str_arr
                line = line.Replace(vbCr, "").Replace(vbLf, "")
                msgbox_ele.SendKeys(line)
                Thread.Sleep(100)
                msgbox_ele.SendKeys(Keys.LeftShift + Keys.Return)
            Next

            Return True

        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try

    End Function

    Private Function Upload_reply_img(img)
        Try
            Dim comment_img_input As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                comment_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("replay_comment_img_input")))
            Else
                comment_img_input = chromeDriver.FindElement(By.CssSelector("div.pmpvxvll.e9r0l795 > ul > li:nth-child(3) > input"))
            End If

            comment_img_input.SendKeys(img)

            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function

    Private Function Submit_reply_comment()
        Try

            Dim msgbox_ele As Object
            If chromeDriver.Url.Contains("comment_id") Then ' reply someone comment
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='回覆']"))
            Else
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label^='留言'] > p"))
            End If

            For i = 0 To 100

                Try
                    chromeDriver.FindElement(By.ClassName("uiScaledImageContainer")) ' wait until upload success
                    msgbox_ele.SendKeys(Keys.Enter)
                    Return True
                Catch ex As Exception
                    Thread.Sleep(100)
                End Try

            Next

            Return False
        Catch ex As Exception
            Debug.WriteLine(ex)
            Return False
        End Try
    End Function


    Private Function Write_post_send_content(content)
        Try
            Dim myURL = chromeDriver.Url
            If myURL.Contains("groups") Then ' If post in group
                Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='留個言吧......']"))
                msgbox_ele.SendKeys(content)
            Else ' personal page
                Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='在想些什麼？']"))
                msgbox_ele.SendKeys(content)
            End If
            Return True

        Catch ex As Exception

            Return False
        End Try

        Return False


    End Function

    Private Function Clear_post_content()
        Try
            Dim myURL = chromeDriver.Url
            Dim msgbox_ele As Object

            If myURL.Contains("groups") Then ' If post in group
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='留個言吧......']"))

            Else ' personal page
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='在想些什麼？']"))
            End If
            msgbox_ele.SendKeys(Keys.LeftControl + "a")
            msgbox_ele.SendKeys(Keys.Delete)
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Private Function Tring_to_upload_img(img_path_str)

        Dim ele1 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_1").ToString)
        Dim ele2 = IsElementPresent(css_selector_config_obj.Item("group_post_img_input_2").ToString)

        If ele1 = False AndAlso ele2 = False Then
            If click_by_aria_label("相片／影片") = False Then
                Return False
            End If
        End If

        Dim upload_img_input As Object

        If IsElementPresent(css_selector_config_obj.Item("group_post_img_input_1").ToString) Then
            Try
                upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_1").ToString))
                upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
                Return True
            Catch ex As Exception
                Return False
            End Try

        Else
            Try
                upload_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("group_post_img_input_2").ToString))
                upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
                Return True
            Catch ex As Exception
                Return False
            End Try

        End If

        Return False

    End Function

    Private Function Upload_img_to_reply(img_str)

        Debug.WriteLine(img_CheckedListBox.Items(0).ToString())
        Dim comment_img_input = chromeDriver.FindElement(By.CssSelector(css_selector_config_obj.Item("replay_comment_img_input")))
        comment_img_input.SendKeys(img_CheckedListBox.Items(0).ToString())

    End Function


    Private Sub Get_url_btn_Click(sender As Object, e As EventArgs) Handles Get_url_btn.Click
        Try
            curr_url_TextBox.Text = chromeDriver.Url
        Catch ex As Exception
            MsgBox("未偵測到Chrome")
        End Try
    End Sub


    Private Sub Insert_Login_Button_Click(sender As Object, e As EventArgs) Handles Insert_login_Button.Click
        Dim fb_email = fb_account_TextBox.Text
        Dim fb_passwd = fb_password_TextBox.Text

        If fb_email = "" OrElse fb_passwd = "" Then
            MsgBox("帳號與密碼不可為空")
        Else
            Insert_to_script("登入", fb_email + ";" + fb_passwd)
        End If

    End Sub

    Private Sub Insert_navigate_to_url_btn_Click(sender As Object, e As EventArgs) Handles Insert_navigate_to_url_btn.Click

        Dim pattern As String
        pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
        If Regex.IsMatch(curr_url_TextBox.Text, pattern) Then
            Insert_to_script("前往", curr_url_TextBox.Text)
        Else
            MsgBox("網址格式錯誤")
        End If

    End Sub

    Private Sub Insert_delay_btn_Click(sender As Object, e As EventArgs) Handles Insert_delay_btn.Click

        Dim rnd_num As New Random()
        Dim hour = wait_hour_NumericUpDown.Value
        Dim minute = wait_minute_NumericUpDown.Value
        Dim second = wait_second_NumericUpDown.Value
        Dim random_sec = rnd_num.Next(-wait_random_second_NumericUpDown.Value, wait_random_second_NumericUpDown.Value)

        Dim total_second = hour * 3600 + minute * 60 + second + random_sec

        Debug.WriteLine(total_second)
        If total_second > 0 Then
            Insert_to_script("等待", total_second)
        Else 'if generated the negative number give default second
            Insert_to_script("等待", "1")
        End If

    End Sub

    Private Sub Clear_script_btn_Click(sender As Object, e As EventArgs) Handles Clear_script_btn.Click
        script_ListView.Items.Clear()
    End Sub

    Private Sub Insert_open_browser_btn_Click(sender As Object, e As EventArgs) Handles Insert_open_browser_btn.Click
        If chrome_RadioButton.Checked = True Then
            used_browser = "Chrome"
            Dim myprofile = profile_path_TextBox.Text
            chrome_profile = myprofile.Split("\")(UBound(myprofile.Split("\")))
        ElseIf firefox_RadioButton.Checked = True Then
            used_browser = "Firefox"
        ElseIf edge_RadioButton.Checked = True Then
            used_browser = "Edge"
        End If
        Insert_to_script("開啟", profile_path_TextBox.Text)
    End Sub

    Private Sub Insert_click_leave_msg_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_leave_msg_btn.Click
        Insert_to_script("點擊", "留個言吧")
    End Sub

    Private Sub Insert_send_content_btn_Click(sender As Object, e As EventArgs) Handles Insert_send_content_btn.Click
        Insert_to_script("發送", content_RichTextBox.Text)
    End Sub

    Private Sub Insert_clear_content_btn_Click(sender As Object, e As EventArgs) Handles Insert_clear_content_btn.Click
        Insert_to_script("清空", "內容")
    End Sub

    Private Sub Insert_click_img_video_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_img_video_btn.Click
        Dim img_path_str As String = ""

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
            Insert_to_script("上載", img_path_str)
        Else
            MsgBox("未勾選任何檔案")
        End If

    End Sub

    Private Sub Insert_submit_post_btn_Click(sender As Object, e As EventArgs) Handles Insert_submit_post_btn.Click
        Insert_to_script("點擊", "發佈")
    End Sub

    Private Sub Insert_close_driver_btn_Click(sender As Object, e As EventArgs) Handles Insert_close_driver_btn.Click
        Insert_to_script("關閉", "瀏覽器")
    End Sub

    Private Sub Insert_click_reply_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_reply_btn.Click
        Insert_to_script("點擊", "回覆/留言")
    End Sub

    Private Sub Insert_reply_comment_btn_Click(sender As Object, e As EventArgs) Handles Insert_reply_comment_btn.Click
        Insert_to_script("回應:內容", reply_content_RichTextBox.Text)
    End Sub

    Private Sub Insert_comment_upload_img_btn_Click(sender As Object, e As EventArgs) Handles Insert_comment_upload_img_btn.Click
        Dim img_path_str As String = ""

        'get selected img path into string 
        If reply_img_CheckedListBox.CheckedItems.Count <> 0 Then

            For i = 0 To reply_img_CheckedListBox.Items.Count - 1
                'We ask if this item is checked or not
                If reply_img_CheckedListBox.GetItemChecked(i) Then
                    img_path_str = reply_img_CheckedListBox.Items(i).ToString()
                End If
            Next

            Insert_to_script("回應:上載", img_path_str)
        Else
            MsgBox("未勾選任何檔案")
        End If
    End Sub




    Private Sub Insert_submit_comment_btn_Click(sender As Object, e As EventArgs) Handles Insert_submit_comment_btn.Click
        Insert_to_script("回應:送出", "送出")
    End Sub
End Class
