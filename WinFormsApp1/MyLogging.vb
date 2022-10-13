Imports Microsoft.VisualBasic
Imports System.IO.File

Module MyLogging

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

        Return log_path + "\log_" + thisTime + ".txt"
    End Function

    Public Sub Write_to_file(file_path As String, content As String)

        Dim thisDate As String = Date.Today.ToString("dd-MM-yyyy")
        Dim thisTime As String = Date.Now.ToString("HH-mm-ss")

        Dim log_file As System.IO.StreamWriter
        log_file = My.Computer.FileSystem.OpenTextFileWriter(file_path, True)
        log_file.WriteLine(thisDate + "," + thisTime + "," + content)
        log_file.Close()
    End Sub


End Module
