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

Public Class Form1

    Dim fileContents As String = System.IO.File.ReadAllText("C:\selenium_file\fb_auth.txt")
    Dim fb_email As String = Split(fileContents)(0)
    Dim fb_passwd As String = Split(fileContents)(1)

    Dim chromeDriver As IWebDriver
    Dim webDriverWait As WebDriverWait

    Dim cursor_x As Integer = 0
    Dim cursor_y As Integer = 0


    'Dim webDriverWait As WebDriverWait


    Private Sub Invoke_Chrome_btn_Click(sender As Object, e As EventArgs) Handles invoke_chrome_btn.Click

        'Try
        'Dim url = chromeDriver.Url
        'MsgBox("chrome A already exist")
        'Exit Sub
        'Catch ex As Exception
        'Debug.WriteLine(ex)
        'End Try

        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New ChromeConfig())


        Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService
        serv.HideCommandPromptWindow = True 'hide cmd

        Dim options = New Chrome.ChromeOptions()
        options.AddArguments("--disable-notifications", "--disable-popup-blocking")

        If pc_RadioButton.Checked = False Then
            Dim dev_model As String = ""
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
        Thread.Sleep(1000)


        'chromeDriver.ExecuteJavaScript("onmousemove = function(e){ mouse.x = e.clientX, mouse.y = e.clientY };")


        driver_close_bnt.Enabled = True
        refresh_url_timer.Enabled = True
        chromeDriver.Navigate.GoToUrl("https://www.facebook.com/")
        'chromeDriver.Navigate.GoToUrl("https://www.google.com.tw/?hl=zh_TW")


        Thread.Sleep(1000)
        chromeDriver.FindElement(By.Name("email")).SendKeys(fb_email)
        Thread.Sleep(500)
        chromeDriver.FindElement(By.Name("pass")).SendKeys(fb_passwd)
        Thread.Sleep(500)
        chromeDriver.FindElement(By.Name("pass")).SendKeys(Keys.Return)
        Thread.Sleep(1000)



    End Sub


    Private Sub Write_post_Click(sender As Object, e As EventArgs) Handles write_a_post_btn.Click

        'Dim postURL As String = "https://www.facebook.com/groups/737807930865755/posts/737820730864475?comment_id=737820784197803"
        'Dim postURL As String = "https://www.facebook.com/ETtoday/posts/pfbid0Z9CFxwaXaUKQLEUtRMU8aqHomsBiygPgLcqzFXnDYoE8eJ9Qu4ZY9yCvK8tAwzbol?comment_id=608850700553438"
        Dim myURL As String = target_url_TextBox.Text

        Dim js_code As String = "document.querySelector(""._1mf._1mj > Span "").innerHTML = ""123"" "


        If myURL.Contains("groups") Then ' If post in group
            chromeDriver.Navigate.GoToUrl(myURL)


            Thread.Sleep(1000)
            Tring_to_upload_img()
            Thread.Sleep(1000)
            chromeDriver.FindElement(By.CssSelector("._1mf._1mj")).SendKeys(content_RichTextBox.Text)
            'Dim msgBox = chromeDriver.FindElement(By.CssSelector("._1mf._1mj"))

            Thread.Sleep(500)
            'chromeDriver.ExecuteJavaScript(js_code)
            '### submit post ###
            'click_by_span_text("發佈")


        Else ' 
            'Dim btn_eles = chromeDriver.FindElements(By.CssSelector("div.oajrlxb2.g5ia77u1.mtkw9kbi.tlpljxtp.qensuy8j.ppp5ayq2.goun2846.ccm00jje.s44p3ltw.mk2mc5f4.rt8b4zig.n8ej3o3l.agehan2d.sk4xxmp2.rq0escxv.nhd2j8a9.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.l9j0dhe7.i1ao9s8h.esuyzwwr.f1sip0of.du4w35lb.btwxx1t3.abiwlrkh.p8dawk7l.lzcic4wl.bp9cbjyn.ue3kfks5.pw54ja7n.uo3d90p7.l82x9zwi.j83agx80.rj1gh0hx.buofh1pr.g5gj957u.taijpn5t.idt9hxom.cxgpxx05.dflh9lhu.sj5x9vvc.scb9dxdr"))
            'btn_eles.ElementAt(1).Click()
            Dim fail_over = False
            Dim msgbox_ele As Object
            chromeDriver.FindElement(By.XPath("//span[contains(text(),'相片／影片')]")).Click()
            Thread.Sleep(2000)
            Try
                'msgbox_ele = chromeDriver.FindElement(By.CssSelector("._1mf._1mj"))
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='在想些什麼？']"))
                msgbox_ele.SendKeys(content_RichTextBox.Text)
            Catch ex As Exception
                fail_over = True
            End Try

            If fail_over Then
                'msgbox_ele = chromeDriver.FindElement(By.CssSelector(".oo9gr5id.lzcic4wl.l9j0dhe7.gsox5hk5.rq0escxv.a8c37x1j.datstx6m.k4urcfbm.notranslate"))
                msgbox_ele = chromeDriver.FindElement(By.CssSelector("div[aria-label$='在想些什麼？']"))
                msgbox_ele.SendKeys(content_RichTextBox.Text)

            End If
            Thread.Sleep(1000)
            'Dim img_upload_input = chromeDriver.FindElement(By.CssSelector("div.rq0escxv.l9j0dhe7.du4w35lb.j83agx80.cbu4d94t.pfnyh3mw.d2edcug0.dflh9lhu.scb9dxdr.aahdfvyu.tvmbv18p.gbw9n0fl.fneq0qzw > input"))

            Dim img_upload_input = chromeDriver.FindElement(By.CssSelector("div.rq0escxv.l9j0dhe7.du4w35lb.j83agx80.cbu4d94t.pfnyh3mw.d2edcug0.dflh9lhu.scb9dxdr.aahdfvyu.tvmbv18p.gbw9n0fl.fneq0qzw > input"))
            'imgupload_ele.SendKeys("C:\Users\Yan\Desktop\testimg.png")
            'img_upload_input.SendKeys("C:\Users\Yan\Desktop\testimg.png")


            'Debug.WriteLine(img_path_str)
            'img_upload_input.SendKeys(img_path_str)

            '### submit post ###
            'chromeDriver.FindElement(By.CssSelector("div.k4urcfbm.discj3wi.dati1w0a.hv4rvrfc.i1fnvgqd.j83agx80.rq0escxv.bp9cbjyn > input")).Click()

        End If



    End Sub

    Private Sub Tring_to_upload_img()
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
        End If

        click_by_span_text("留個言吧……")
        Thread.Sleep(2000)

        Dim ele1 = IsElementPresent("div.om3e55n1.g4tp4svg.bdao358l.alzwoclg.cqf1kptm.gvxzyvdx.thmcm15y.cgu29s5g.i15ihif8.dnr7xe2t.q46jt4gp.r5g9zsuq > div > div:nth-child(1) > input")
        Dim ele2 = IsElementPresent("#toolbarLabel + div > div > input")

        If ele1 = False AndAlso ele2 = False Then
            click_by_aria_label("相片／影片")
        End If

        Thread.Sleep(2000)
        'Dim upload_img_input = chromeDriver.FindElement(By.CssSelector("div.fwlpnqze.r5g9zsuq.b0eko5f3.q46jt4gp.p9ctufpz.rj0o91l8.sl27f92c.alzwoclg.bdao358l.jgcidaqu.ta68dy8c.kpwa50dg.m0cukt09.h8391g91.qykh3frn.i0v5kuzt.lkznwk7v.gxnvzty1.k0kqjr44.i85zmo3j > div.alzwoclg > div:nth-child(1) > input"))
        'Dim upload_img_input = chromeDriver.FindElement(By.CssSelector("#toolbarLabel + div > div > input"))
        Dim upload_img_input As Object

        If IsElementPresent("div.om3e55n1.g4tp4svg.bdao358l.alzwoclg.cqf1kptm.gvxzyvdx.thmcm15y.cgu29s5g.i15ihif8.dnr7xe2t.q46jt4gp.r5g9zsuq > div > div:nth-child(1) > input") Then
            upload_img_input = chromeDriver.FindElement(By.CssSelector("div.om3e55n1.g4tp4svg.bdao358l.alzwoclg.cqf1kptm.gvxzyvdx.thmcm15y.cgu29s5g.i15ihif8.dnr7xe2t.q46jt4gp.r5g9zsuq > div > div:nth-child(1) > input"))
            upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
        Else
            upload_img_input = chromeDriver.FindElement(By.CssSelector("#toolbarLabel + div > div > input"))
            upload_img_input.SendKeys(img_path_str) ' if muti img use "& vbLf &" to join the img path
        End If


    End Sub


    Private Sub Get_Groups_Click(sender As Object, e As EventArgs) Handles get_groups_btn.Click

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
            Group_ListView.Items.Add(group_name_classes.ElementAt(i - 1).GetAttribute("innerHTML"), 100)
            Group_ListView.Items(i - 1).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
        Next

    End Sub

    Private Sub get_groups_from_m_Click(sender As Object, e As EventArgs) Handles get_groups_from_m.Click

        Dim curr_row As Integer = 0

        '### Find manage groups ###
        chromeDriver.Navigate.GoToUrl("https://m.facebook.com/groups_browse/your_groups/manage/")
        Thread.Sleep(3000)
        Dim pre_counter As Integer = 0

        While True 'Scroll to the bottom
            Dim my_counter As Integer = chromeDriver.FindElements(By.CssSelector(".h3z9dlai.ld7irhx5.pbevjfx6.igjjae4c")).Count
            If my_counter = pre_counter Then
                Exit While
            End If
            chromeDriver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);")
            pre_counter = my_counter
            Thread.Sleep(1000)
        End While


        'Get manage groups and add to the listview
        Dim mng_group_name_classes = chromeDriver.FindElements(By.CssSelector(".h3z9dlai.ld7irhx5.pbevjfx6.igjjae4c"))
        Dim mng_group_url_classes = chromeDriver.FindElements(By.CssSelector("div > div > div > div > div._2pip > div > a"))
        For i As Integer = 0 To mng_group_name_classes.Count - 1
            'Debug.WriteLine(group_classes.ElementAt(i).GetAttribute("href"))
            Group_ListView.Items.Add(mng_group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            Group_ListView.Items(curr_row).SubItems.Add(mng_group_url_classes.ElementAt(i).GetAttribute("href"))
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
            Group_ListView.Items.Add(group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            Group_ListView.Items(curr_row).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
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



    Private Sub Reply_comment_bnt_Click(sender As Object, e As EventArgs) Handles reply_comment_bnt.Click
        Dim str_arr() As String = content_RichTextBox.Text.Split(vbLf)
        Dim msgbox_ele = chromeDriver.FindElement(By.CssSelector("._1mf._1mj"))
        For Each line As String In str_arr
            line = line.Replace(vbCr, "").Replace(vbLf, "")
            msgbox_ele.SendKeys(line)
            Thread.Sleep(100)
            msgbox_ele.SendKeys(Keys.LeftShift + Keys.Return)
            Thread.Sleep(100)
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'driver_close_bnt.Enabled = False
        target_url_TextBox.Text = "https://www.facebook.com/groups/737807930865755"
        Group_ListView.View = View.Details
        Group_ListView.GridLines = True
        Group_ListView.FullRowSelect = True
        Group_ListView.Columns.Add("GroupName", 100)
        Group_ListView.Columns.Add("URL", 800)


        For Each Dir As String In My.Computer.FileSystem.GetDirectories(My.Computer.FileSystem.CurrentDirectory + "\Chrome")
            'Debug.WriteLine("DIR : " + Dir)
            'chromedriver_ListBox.Items.Add(Dir + "\X64\chromedriver.exe")
            For Each driver_dir As String In My.Computer.FileSystem.GetDirectories(Dir)
                'Debug.WriteLine(driver_dir)
                chromedriver_ListBox.Items.Add(driver_dir + "\chromedriver.exe")

            Next

        Next



        render_img_listbox()

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
        Catch ex As System.NullReferenceException
            Debug.WriteLine(ex)
        End Try

    End Sub

    Private Sub replace_str_btn_Click(sender As Object, e As EventArgs) Handles replace_str_btn.Click
        Dim compare_str = compare_str_textbox.Text
        Dim replace_str = replace_str_textbox.Text

        For Each item As ListViewItem In Group_ListView.Items
            Debug.WriteLine(item.SubItems(1))
            item.SubItems(1).Text = item.SubItems(1).Text.Replace(compare_str, replace_str)
        Next


        'Debug.WriteLine(Group_ListView)


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

    Private Sub click_by_aria_label(str As String)
        Try
            chromeDriver.FindElement(By.CssSelector("div[aria-label$='" + str + "']")).Click()
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

    Private Sub click_by_span_text(str As String)
        Try
            chromeDriver.FindElement(By.XPath("//span[contains(text(),'" + str + "')]")).Click()
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

    Function IsElementPresent(locatorKey As String) As Boolean
        Try
            chromeDriver.FindElement(By.CssSelector(locatorKey))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


End Class
