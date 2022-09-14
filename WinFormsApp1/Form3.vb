Imports System.Threading
Public Class ScriptEditor_Form

    Dim current_profile As String = "cmd"
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
                If cmd_array.Length = 2 Then
                    Form1.open_Chrome(cmd_array(1).Replace("%20", " "))
                    current_profile = cmd_array(1).Split("\")(UBound(cmd_array(1).Split("\")))
                    print_to_output(current_profile)
                Else
                    current_profile = "cmd"
                    Form1.open_Chrome("")
                    print_to_output("open chrome")

                End If


            Case "quit_chrome"
                Form1.quit_chrome()
                print_to_output("quit chrome")
            Case "login_fb"
                If cmd_array.Length = 3 Then
                    print_to_output("login with " & cmd_array(1))
                    Form1.login_fb(cmd_array(1), cmd_array(2))
                Else
                    print_to_output("error invalid parameter")
                End If
            Case "navigate_to"
                If cmd_array.Length = 2 Then
                    print_to_output("navigate to " & cmd_array(1))
                    Form1.navigate_GoToUrl(cmd_array(1).Replace("%20", " "))
                Else
                    print_to_output("error invalid parameter")
                End If
            Case "write_post"
                If cmd_array.Length = 2 Then
                    print_to_output("write post to " & cmd_array(1))
                    Form1.write_post(cmd_array(1).Replace("%20", " "))
                Else
                    print_to_output("error invalid parameter")
                End If

            Case Else
                print_to_output("error unknow command")
        End Select



    End Sub

    Private Sub print_to_output(content As String)
        script_output_RichTextBox.AppendText(current_profile + ": " + content & vbCrLf)
    End Sub

    Private Sub show_help()
        script_output_RichTextBox.Text = "Usage: [command] [parameter1] [parameter2]...
        open_chrome [profile]" & vbTab & "-open chrome with profile or not
        quit_chrome" & vbTab & "-close chrome and quit chrome driver
        navigate_to [url]" & vbTab & "-navigate to url
        login_fb [email] [password]" & vbTab & "-login facebook"
    End Sub

    Private Sub show_help_btn_Click(sender As Object, e As EventArgs) Handles show_help_btn.Click
        show_help()
    End Sub
End Class