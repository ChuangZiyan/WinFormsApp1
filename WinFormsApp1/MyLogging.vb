Imports Microsoft.VisualBasic
Imports System.IO.File
Public Class MyLogging

    Public Function SayHello()
        Debug.WriteLine("hello")
        Return 0
    End Function


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


End Class
