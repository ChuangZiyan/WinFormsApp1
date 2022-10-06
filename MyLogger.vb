Imports Microsoft.VisualBasic

Public Class MyLogger

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

End Class
