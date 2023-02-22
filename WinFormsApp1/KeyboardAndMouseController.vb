
Imports System.Threading

Public Class KeyboardAndMouseController


    Dim record = False


    Public WithEvents MouseHook As New MouseHook
    Public Sub MouseHook_Mouse_Left_Down(e As Point) Handles MouseHook.Mouse_Left_Down
        Dim MousePosition = Cursor.Position
        'Debug.WriteLine(MousePosition)
        Click_All_Window(MousePosition.X, MousePosition.Y)

        If record Then
            Insert_to_script("系統點擊:左鍵", "0;" + Str(MousePosition.X) + ":" + Str(MousePosition.Y))
        End If

    End Sub


    Public Sub MouseHook_Mouse_Right_Down(e As Point) Handles MouseHook.Mouse_Right_Down
        If Not record Then
            Exit Sub
        End If
        Dim MousePosition = Cursor.Position
        Insert_to_script("系統點擊:右鍵", "0;" + Str(MousePosition.X) + ":" + Str(MousePosition.Y))
    End Sub


    Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)

    Public Sub Test_System_Mouse_Position_OnClick()
        If Form1.Cursor_X_Position_TextBox.Text <> "" And Form1.Cursor_Y_Position_TextBox.Text <> "" Then
            Cursor.Position = New Point(CInt(Form1.Cursor_X_Position_TextBox.Text), CInt(Form1.Cursor_Y_Position_TextBox.Text))
            mouse_event(&H2, 0, 0, 0, 0)
            mouse_event(&H4, 0, 0, 0, 0)
        End If

    End Sub

    Public Function System_Mouse_OnClick_By_Position(button As String, x As Integer, y As Integer)
        Try
            Cursor.Position = New Point(x, y)
            If button = "left" Then
                mouse_event(&H2, 0, 0, 0, 0) 'left btn down 
                mouse_event(&H4, 0, 0, 0, 0) 'left btn up
            ElseIf button = "right" Then
                mouse_event(&H8, 0, 0, 0, 0) 'right btn down
                mouse_event(&H10, 0, 0, 0, 0) 'right btn up
            End If

            Return True

        Catch ex As Exception
            Return False
        End Try


    End Function

    Public WithEvents kbHook As New KeyboardHook
    Dim LControlKey_KeyDown = False
    Dim LMenu_KeyDown = False
    Dim F1_KeyDown = False
    Dim F2_KeyDown = False


    Public myKeyboardHook = True

    Public Sub kbHook_KeyDown(ByVal Key As System.Windows.Forms.Keys) Handles kbHook.KeyDown
        If myKeyboardHook = False Then
            Exit Sub
        End If
        Debug.WriteLine("GO1")
        'Debug.WriteLine("keydown " + Key.ToString)

        If LControlKey_KeyDown AndAlso Key.ToString = "V" Then
            Debug.WriteLine("PAUSE")
            myKeyboardHook = False
            Thread.Sleep(1000)
            SendKey_To_All_Window(Key.ToString)
            myKeyboardHook = True
        End If



        Select Case Key.ToString
            Case "LControlKey"
                LControlKey_KeyDown = True
            Case "LMenu"
                LMenu_KeyDown = True
            Case "F1"
                F1_KeyDown = True
            Case "F2"
                F2_KeyDown = True
        End Select

        If LControlKey_KeyDown And LMenu_KeyDown Then
            Form1.Cursor_X_Position_TextBox.Text = Form1.Cursor_X_Position_Label.Text
            Form1.Cursor_Y_Position_TextBox.Text = Form1.Cursor_Y_Position_Label.Text
        End If


    End Sub
    Public Sub kbHook_KeyUp(ByVal Key As System.Windows.Forms.Keys) Handles kbHook.KeyUp

        If myKeyboardHook = False Then
            Exit Sub
        End If
        Debug.WriteLine("GO2")

        Dim myKey = Key.ToString

        If record And myKey <> "LControlKey" Then

            Insert_To_Script_By_Record(Key.ToString)

        End If

        If LControlKey_KeyDown And F1_KeyDown Then
            record = True
        End If

        If LControlKey_KeyDown And F2_KeyDown Then
            record = False
        End If

        Select Case Key.ToString
            Case "LControlKey"
                LControlKey_KeyDown = False
            Case "LMenu"
                LMenu_KeyDown = False
            Case "F1"
                F1_KeyDown = False
            Case "F2"
                F2_KeyDown = False
        End Select


    End Sub


    Public Sub Insert_To_Script_By_Record(scnd_key)

        Dim firstKey = ""
        Dim secondKey = scnd_key

        If LControlKey_KeyDown Then
            firstKey = "CTRL"
        ElseIf LMenu_KeyDown Then
            firstKey = "ALT"
        End If

        If firstKey = "CTRL" And scnd_key = "F1" Then
            Exit Sub
        ElseIf firstKey = "CTRL" And scnd_key = "F2" Then
            Exit Sub
        End If

        Insert_to_script("系統發送:按鍵", firstKey + "+" + secondKey)
    End Sub


End Class
