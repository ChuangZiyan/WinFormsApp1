Imports System.Collections.ObjectModel
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports OpenQA.Selenium

Public Class CookieEditorDialog1

    Public Class myCookie
        Public Property domain As String
        Public Property name As String
        Public Property value As String
        Public Property path As String

    End Class



    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Debug.WriteLine(CookieRichTextBox.Text)
        Try
            Dim jsonText = CookieRichTextBox.Text
            Dim cookie_list As List(Of myCookie) = JsonConvert.DeserializeObject(Of List(Of myCookie))(jsonText)

            If cookie_list Is Nothing Then
                MsgBox("Json Format Error")
                Exit Sub
            End If

            For Each ck In cookie_list
                'Debug.WriteLine("#### " + ck.domain + " " + ck.name + " " + ck.value)
                Dim cookie As New OpenQA.Selenium.Cookie(ck.name, ck.value, ck.domain, ck.path, DateTime.Now.AddDays(365))
                Form1.myWebDriver.chromeDriver.Manage.Cookies.AddCookie(cookie)

            Next

            MsgBox("設定成功")
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
            CookieRichTextBox.Clear()
        Catch ex As Exception
            MsgBox("Json Format Error")
        End Try

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
        CookieRichTextBox.Clear()
    End Sub

    Private Sub Read_Curr_Cookie_Button_Click(sender As Object, e As EventArgs) Handles Read_Curr_Cookie_Button.Click
        Try
            Debug.WriteLine("Read Cookie")
            Dim cookies As ReadOnlyCollection(Of OpenQA.Selenium.Cookie) = Form1.myWebDriver.chromeDriver.Manage().Cookies.AllCookies
            Dim cookieList As New List(Of myCookie)

            ' 顯示每個 cookie 的相關信息
            For Each cookie As OpenQA.Selenium.Cookie In cookies
                Debug.WriteLine($"Cookie Name: {cookie.Name}, Value: {cookie.Value}")
                Dim myCookieObj As New myCookie()
                myCookieObj.name = cookie.Name
                myCookieObj.value = cookie.Value
                myCookieObj.domain = cookie.Domain
                myCookieObj.path = cookie.Path
                cookieList.Add(myCookieObj)
            Next

            Dim jsonStr As String = JsonConvert.SerializeObject(cookieList, Formatting.Indented)

            CookieRichTextBox.Text = jsonStr

        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

End Class
