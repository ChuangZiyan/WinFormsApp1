Imports System.Threading
Public Class ScriptEditor_Form
    Private Sub run_script_btn_Click(sender As Object, e As EventArgs) Handles run_script_btn.Click
        script_output_RichTextBox.Clear()
        Dim script_text = script_editor_richtextbox.Text
        'script_log_RichTextBox.Text = script_text
        For Each cmd As String In script_editor_richtextbox.Lines
            'script_log_RichTextBox.AppendText("Cmd:" + cmd & vbCrLf)
            command_routing(cmd)
        Next

    End Sub

    Private Sub command_routing(cmd As String)
        Thread.Sleep(2000) ' for test
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
        open_chrome" & vbTab & vbTab & "-open chrome
        quit_chrome" & vbTab & vbTab & "-close chrome and quit chrome driver
        navigate_to [url]" & vbTab & vbTab & "-navigate to url"
    End Sub

    Private Sub show_help_btn_Click(sender As Object, e As EventArgs) Handles show_help_btn.Click
        show_help()
    End Sub
End Class