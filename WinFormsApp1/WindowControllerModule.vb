Imports System.Reflection.Emit
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

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

    Public Function get_All__Chrome_hWnd() As ArrayList

        chromeWindows.Clear() ' clear dic.
        EnumWindows(AddressOf ClassesByName, IntPtr.Zero) ' enum windows w/classname "Chrome_WidgetWin_1".

        Dim hWndArrayList As ArrayList = New ArrayList()
        ' display contents
        If chromeWindows.Count = 0 Then
            MessageBox.Show("None found, list is empty!")
        Else ' do something with the results,...
            For Each chrome In chromeWindows
                Debug.WriteLine("hWnd={0}, Title={1}", chrome.Key, chrome.Value)
                If chrome.Value.Contains("Google Chrome") Then
                    hWndArrayList.Add(chrome.Key)
                End If

            Next
        End If
        Return hWndArrayList
    End Function


    Public Sub perform_Window_Layout()
        Dim hWndArrayList = get_All__Chrome_hWnd()
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height

        Debug.WriteLine("screenWidth : {0}", screenWidth)
        Debug.WriteLine("screenHeight : {0}", screenHeight)

        If hWndArrayList.Count = 1 Then
            SetWindowPos(hWndArrayList(0), 0, 0, 0, screenWidth, screenHeight, &H40)
        ElseIf hWndArrayList.Count = 2 Then
            Dim pos_left = 0
            Dim pos_top = 0

            For Each hWnd In hWndArrayList
                Debug.WriteLine("set : pos_left:{0}  pos_top:{1} screenWidth:{2} screenHeight:{3}", pos_left, pos_top, screenWidth / 2, screenHeight)
                SetWindowPos(hWnd, 0, pos_left, pos_top, screenWidth / 2, screenHeight, &H40)
                pos_left += screenWidth / 2
            Next
        ElseIf hWndArrayList.Count <= 4 Then
            Dim pos_left = 0
            Dim pos_top = 0
            Dim window_counter = 0

            For Each hWnd In hWndArrayList
                window_counter += 1
                Debug.WriteLine("set : pos_left:{0}  pos_top:{1} screenWidth:{2} screenHeight:{3}", pos_left, pos_top, screenWidth / 2, screenHeight / 2)
                SetWindowPos(hWnd, 0, pos_left, pos_top, screenWidth / 2, screenHeight / 2, &H40)
                pos_left += screenWidth / 2

                If window_counter = 2 Then
                    pos_left = 0
                    pos_top += screenHeight / 2
                End If
            Next

        ElseIf hWndArrayList.Count <= 6 Then
            Dim pos_left = 0
            Dim pos_top = 0
            Dim window_counter = 0

            For Each hWnd In hWndArrayList
                window_counter += 1
                Debug.WriteLine("set : pos_left:{0}  pos_top:{1} screenWidth:{2} screenHeight:{3}", pos_left, pos_top, screenWidth / 3, screenHeight / 2)
                SetWindowPos(hWnd, 0, pos_left, pos_top, screenWidth / 3, screenHeight / 2, &H40)
                pos_left += screenWidth / 3

                If window_counter = 3 Then
                    pos_left = 0
                    pos_top += screenHeight / 2
                End If
            Next
        End If
    End Sub



    Public Sub Set_All_Window_Size(width As Integer, height As Integer)


        Dim chromeWindows As ArrayList = get_All__Chrome_hWnd()

        For Each chrome In chromeWindows
            Dim rct As RECT
            GetWindowRect(chrome, rct)
            Debug.WriteLine("Left:" & rct.Left)
            Debug.WriteLine("Top:" & rct.Top)
            Debug.WriteLine("Right:" & rct.Right)
            Debug.WriteLine("Bottom:" & rct.Bottom)
            SetWindowPos(chrome, 0, rct.Left, rct.Top, width, height, &H40)
            'Exit For
        Next

    End Sub

    Public Sub Overlap_All_Window(X As Integer, Y As Integer)
        Dim chromeWindows As ArrayList = get_All__Chrome_hWnd()
        For Each chrome In chromeWindows
            Dim rct As RECT
            GetWindowRect(chrome, rct)
            Debug.WriteLine("Left:" & rct.Left)
            Debug.WriteLine("Top:" & rct.Top)
            Debug.WriteLine("Right:" & rct.Right)
            Debug.WriteLine("Bottom:" & rct.Bottom)
            SetWindowPos(chrome, 0, X, Y, rct.Right - rct.Left, rct.Bottom - rct.Top, &H40)
            'Exit For
        Next

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





    '############### Sample code for test  ###################





    Public Const WM_LBUTTONDOWN = &H201
    Public Const WM_LBUTTONUP = &H202

    Public Function test_All__Chrome_hWnd() As ArrayList
        chromeWindows.Clear() ' clear dic.
        EnumWindows(AddressOf ClassesByName, IntPtr.Zero) ' enum windows w/classname "Chrome_WidgetWin_1".

        Dim pos_x = CInt(Form1.pos_X_TextBox1.Text)
        Dim pos_y = CInt(Form1.pos_Y_TextBox1.Text)

        Dim hWndArrayList As ArrayList = New ArrayList()
        ' display contents
        If chromeWindows.Count = 0 Then
            MessageBox.Show("None found, list is empty!")
        Else ' do something with the results,...
            For Each chrome In chromeWindows
                Debug.WriteLine("hWnd={0}, Title={1}", chrome.Key, chrome.Value)
                If chrome.Value.Contains("Google Chrome") Then

                    PostMessage(chrome.Key, WM_LBUTTONDOWN, 0, MAKELPARAM(pos_x, pos_y))    '點下滑鼠左鍵 
                    PostMessage(chrome.Key, WM_LBUTTONUP, 0, MAKELPARAM(pos_x, pos_y))

                End If

            Next
        End If
        Return hWndArrayList
    End Function

    Public Function MAKELPARAM(ByVal l As Integer, ByVal h As Integer) As Long '仿C# MAKELPARAM()函數 
        Dim r As Integer = l + (h << 16)
        Return (r)
    End Function

    <DllImport("user32")>
    Public Function PostMessage(ByVal hwnd As IntPtr, ByVal wMsg As UInt32, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function



    <DllImport("user32.dll", EntryPoint:="SendMessageW")>
    Private Function SendMessageW(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function





End Module
