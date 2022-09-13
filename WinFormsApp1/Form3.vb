Imports System.Threading
Public Class ScriptEditor_Form
    Private Sub run_script_btn_Click(sender As Object, e As EventArgs) Handles run_script_btn.Click
        script_output_RichTextBox.Clear()
        For Each cmd As String In script_editor_richtextbox.Lines
            'script_log_RichTextBox.AppendText("Cmd:" + cmd & vbCrLf)
            command_routing(cmd)
        Next

    End Sub

    Private Sub command_routing(cmd As String)
        Thread.Sleep(2000) ' for testing
        Dim cmd_array() As String = Split(cmd)

        Select Case cmd_array(0)
            Case "show_help"
                show_help()
            Case "open_chrome"
                Form1.open_Chrome()
                script_output_RichTextBox.AppendText("cmd: open chrome" & vbCrLf)
            Case "quit_chrome"
                Form1.quit_chrome()
                script_output_RichTextBox.AppendText("cmd: quit chrome" & vbCrLf)
            Case "login_fb"
                If cmd_array.Length = 3 Then
                    script_output_RichTextBox.AppendText("cmd: login with " & cmd_array(1) & vbCrLf)
                    Form1.login_fb(cmd_array(1), cmd_array(2))
                Else
                    script_output_RichTextBox.AppendText("error: invalid parameter" & vbCrLf)
                End If
            Case "navigate_to"
                If cmd_array.Length = 2 Then
                    script_output_RichTextBox.AppendText("cmd: navigate to " & cmd_array(1) & vbCrLf)
                    Form1.navigate_GoToUrl(cmd_array(1))
                Else
                    script_output_RichTextBox.AppendText("error: invalid parameter" & vbCrLf)
                End If
            Case Else
                script_output_RichTextBox.AppendText("error: unknow command" & vbCrLf)
        End Select



    End Sub

    Private Sub show_help()
        script_output_RichTextBox.Text = "Usage: [command] [parameter1] [parameter2]...
        open_chrome" & vbTab & "-open chrome
        quit_chrome" & vbTab & "-close chrome and quit chrome driver
        navigate_to [url]" & vbTab & "-navigate to url
        login_fb [email] [password]" & vbTab & "-login facebook"
    End Sub

    Private Sub show_help_btn_Click(sender As Object, e As EventArgs) Handles show_help_btn.Click
        show_help()
    End Sub
End Class