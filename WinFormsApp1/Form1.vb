﻿Imports System
Imports System.Threading
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.Extensions
Imports OpenQA.Selenium.Support.UI

Public Class Form1

    Dim fb_email As String = "yan18954@gmail.com"
    Dim fb_passwd As String = "aa147258369"
    Dim chromeDriver As IWebDriver
    Dim webDriverWait As WebDriverWait



    Private Sub Invoke_Chrome_btn_Click(sender As Object, e As EventArgs) Handles invoke_chrome_btn.Click
        Dim options = New Chrome.ChromeOptions()
        options.AddArguments("--disable-notifications")
        chromeDriver = New ChromeDriver(options)
        Thread.Sleep(1000)
        driver_close_bnt.Enabled = True
        chromeDriver.Navigate.GoToUrl("https://www.facebook.com/")


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
        Dim myURL As String = url_TextBox.Text

        Dim js_code As String = "document.querySelector(""._1mf._1mj > Span "").innerHTML = ""123"" "

        If myURL.Contains("groups") Then ' If post in group
            chromeDriver.Navigate.GoToUrl(myURL)
            Thread.Sleep(3000)
            chromeDriver.FindElement(By.XPath("//span[contains(text(),'留個言吧……')]")).Click()
            Thread.Sleep(2000)
            Dim upload_img_input = chromeDriver.FindElement(By.CssSelector("div.dwxx2s2f.dicw6rsg.kady6ibp.rs0gx3tq > input"))
            upload_img_input.SendKeys("C:\Users\Yan\Desktop\testimg.png" & vbLf & "C:\Users\Yan\Desktop\testimg.png") ' if muti img use "& vbLf &" to join the img path
            Thread.Sleep(1000)
            chromeDriver.FindElement(By.CssSelector("._1mf._1mj")).SendKeys(content_RichTextBox.Text)
            'Dim msgBox = chromeDriver.FindElement(By.CssSelector("._1mf._1mj"))

            Thread.Sleep(500)
            'chromeDriver.ExecuteJavaScript(js_code)
            '### submit post ###
            chromeDriver.FindElement(By.CssSelector("div.rq0escxv.l9j0dhe7.du4w35lb.j83agx80.cbu4d94t.d2edcug0.hpfvmrgz.buofh1pr.g5gj957u.ph5uu5jm.b3onmgus.e5nlhep0.ecm0bbzt.mg4g778l > div")).Click()

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
            Dim img_upload_input = chromeDriver.FindElement(By.CssSelector("div.rq0escxv.l9j0dhe7.du4w35lb.j83agx80.cbu4d94t.pfnyh3mw.d2edcug0.dflh9lhu.scb9dxdr.aahdfvyu.tvmbv18p.gbw9n0fl.fneq0qzw > input"))

            'imgupload_ele.SendKeys("C:\Users\Yan\Desktop\testimg.png")
            img_upload_input.SendKeys("C:\Users\Yan\Desktop\testimg.png")


            '### submit post ###
            'chromeDriver.FindElement(By.CssSelector("div.k4urcfbm.discj3wi.dati1w0a.hv4rvrfc.i1fnvgqd.j83agx80.rq0escxv.bp9cbjyn > input")).Click()

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
            Dim my_counter As Integer = chromeDriver.FindElements(By.CssSelector("div.ow4ym5g4.auili1gw.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.oygrvhab.cxmmr5t8.hcukyx3x.kvgmc6g5.tgvbjcpo.hpfvmrgz.qt6c0cv9.rz4wbd8a.a8nywdso.jb3vyjys.du4w35lb.bp9cbjyn.ns4p8fja.btwxx1t3.l9j0dhe7 > div > div > div > div:nth-child(1) > span > span > span")).Count
            If my_counter = pre_counter Then
                Exit While
            End If

            chromeDriver.ExecuteJavaScript("document.getElementsByClassName(""rpm2j7zs k7i0oixp gvuykj2m j83agx80 cbu4d94t ni8dbmo4 du4w35lb q5bimw55 ofs802cu pohlnb88 dkue75c7 mb9wzai9 d8ncny3e buofh1pr g5gj957u tgvbjcpo l56l04vs r57mb794 kh7kg01d eg9m0zos c3g1iek1 l9j0dhe7 k4xni2cv"")[0].scroll(0," & scroll_x_value & ")")
            pre_counter = my_counter
            Thread.Sleep(1000)
        End While


        Dim group_url_classes = chromeDriver.FindElements(By.CssSelector("a.oajrlxb2.gs1a9yip.g5ia77u1.mtkw9kbi.tlpljxtp.qensuy8j.ppp5ayq2.goun2846.ccm00jje.s44p3ltw.mk2mc5f4.rt8b4zig.n8ej3o3l.agehan2d.sk4xxmp2.rq0escxv.nhd2j8a9.mg4g778l.pfnyh3mw.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.tgvbjcpo.hpfvmrgz.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.l9j0dhe7.i1ao9s8h.esuyzwwr.f1sip0of.du4w35lb.btwxx1t3.abiwlrkh.p8dawk7l.lzcic4wl.ue3kfks5.pw54ja7n.uo3d90p7.l82x9zwi.a8c37x1j"))
        Dim group_name_classes = chromeDriver.FindElements(By.CssSelector("div.ow4ym5g4.auili1gw.rq0escxv.j83agx80.buofh1pr.g5gj957u.i1fnvgqd.oygrvhab.cxmmr5t8.hcukyx3x.kvgmc6g5.tgvbjcpo.hpfvmrgz.qt6c0cv9.rz4wbd8a.a8nywdso.jb3vyjys.du4w35lb.bp9cbjyn.ns4p8fja.btwxx1t3.l9j0dhe7 > div > div > div > div:nth-child(1) > span > span > span"))
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
        driver_close_bnt.Enabled = False
        chromeDriver.Close()
        chromeDriver.Quit()

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
        driver_close_bnt.Enabled = False
        url_TextBox.Text = "https://www.facebook.com/groups/737807930865755"
        Group_ListView.View = View.Details
        Group_ListView.GridLines = True
        Group_ListView.FullRowSelect = True
        Group_ListView.Columns.Add("GroupName", 100)
        Group_ListView.Columns.Add("URL", 800)
    End Sub

End Class
