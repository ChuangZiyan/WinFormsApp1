Imports Microsoft.VisualBasic
Imports System.IO.File
Imports WinFormsApp1.Form1
Public Class MyLogging

    Public Function SayHello()
        Debug.WriteLine("hello")
        Return 0
    End Function


    Public Function Get_NewLogFile_dir()
        Dim thisDate As String = Date.Today.ToString("dd-MM-yyyy")
        Dim thisTime As String = Date.Now.ToString("HH-mm-ss")
        Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs\" + thisDate
        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If

    End Function

    Public Sub Log_to_file(newFile As Boolean, content As String)

        Dim thisDate As String = Date.Today.ToString("dd-MM-yyyy")
        Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs\" + thisDate
        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If

        If newFile Then
            Debug.WriteLine("write into new file")
        Else
            Dim log_file As System.IO.StreamWriter
            log_file = My.Computer.FileSystem.OpenTextFileWriter(log_path + "\selenium_log.txt", True)
            log_file.WriteLine(content)
            log_file.Close()

        End If


    End Sub


End Class
