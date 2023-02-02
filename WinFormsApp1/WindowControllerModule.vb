Imports System.Runtime.InteropServices
Imports System.Text

Module WindowControllerModule

    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure



    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Function GetWindowText(ByVal hwnd As IntPtr, ByVal lpString As StringBuilder, ByVal cch As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Function GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As StringBuilder, ByVal nMaxCount As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function


    Private chromeWindows As New Dictionary(Of IntPtr, String)
    Declare Auto Function SetWindowPos Lib "user32" (ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean


    Private Declare Function EnumWindows Lib "user32" (ByVal lpEnumFunc As EnumWindowsProcDelegate, ByVal lParam As IntPtr) As Boolean
    Private Delegate Function EnumWindowsProcDelegate(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean


    Public Sub Set_All_Window_Size(width As Integer, height As Integer)
        chromeWindows.Clear() ' clear dic.
        EnumWindows(AddressOf ClassesByName, IntPtr.Zero) ' enum windows w/classname "Chrome_WidgetWin_1".
        ' display contents
        If chromeWindows.Count = 0 Then
            MessageBox.Show("None found, list is empty!")
        Else ' do something with the results,...
            For Each chrome In chromeWindows
                Debug.WriteLine("hWnd={0}, Title={1}", chrome.Key, chrome.Value)
                Dim rct As RECT
                GetWindowRect(chrome.Key, rct)
                Debug.WriteLine("Left:" & rct.Left)
                Debug.WriteLine("Top:" & rct.Top)
                Debug.WriteLine("Right:" & rct.Right)
                Debug.WriteLine("Bottom:" & rct.Bottom)
                SetWindowPos(chrome.Key, 0, rct.Left, rct.Top, width, height, &H40)
                Exit For
            Next
        End If
    End Sub

    Public Sub Overlap_All_Window(X As Integer, Y As Integer)
        chromeWindows.Clear() ' clear dic.
        EnumWindows(AddressOf ClassesByName, IntPtr.Zero) ' enum windows w/classname "Chrome_WidgetWin_1".
        ' display contents
        If chromeWindows.Count = 0 Then
            MessageBox.Show("None found, list is empty!")
        Else ' do something with the results,...
            For Each chrome In chromeWindows
                Debug.WriteLine("hWnd={0}, Title={1}", chrome.Key, chrome.Value)
                Dim rct As RECT
                GetWindowRect(chrome.Key, rct)
                Debug.WriteLine("Left:" & rct.Left)
                Debug.WriteLine("Top:" & rct.Top)
                Debug.WriteLine("Right:" & rct.Right)
                Debug.WriteLine("Bottom:" & rct.Bottom)
                SetWindowPos(chrome.Key, 0, X, Y, rct.Right - rct.Left, rct.Bottom - rct.Top, &H40)
                Exit For
            Next
        End If
    End Sub

    ' Helper/API Funcs...
    Private Function ClassesByName(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean
        If ClassName(hWnd) = "Chrome_WidgetWin_1" Then 'Google Chrome main window class name.
            Dim capText As New StringBuilder(255) ' get title bar txt/caption text
            If GetWindowText(hWnd, capText, capText.Capacity) > 0 Then ' only add if has text.
                chromeWindows.Add(hWnd, capText.ToString) ' add window handle and caption text to dic.
            End If
        End If
        Return True
    End Function

    Private Function ClassName(ByVal hWnd As IntPtr) As String
        Dim sbClassName As New StringBuilder(256)
        GetClassName(hWnd, sbClassName, 256)
        Return sbClassName.ToString
    End Function


    Public Sub find_all_window()
        chromeWindows.Clear() ' clear dic.
        EnumWindows(AddressOf ClassesByName, IntPtr.Zero) ' enum windows w/classname "Chrome_WidgetWin_1".
        ' display contents
        If chromeWindows.Count = 0 Then
            MessageBox.Show("None found, list is empty!")
        Else ' do something with the results,...
            For Each chrome In chromeWindows
                Debug.WriteLine("hWnd={0}, Title={1}", chrome.Key, chrome.Value)
                Dim rct As RECT
                GetWindowRect(chrome.Key, rct)
                Debug.WriteLine("Left:" & rct.Left)
                Debug.WriteLine("Top:" & rct.Top)
                Debug.WriteLine("Right:" & rct.Right)
                Debug.WriteLine("Bottom:" & rct.Bottom)
                Exit For
            Next
        End If
    End Sub

End Module
