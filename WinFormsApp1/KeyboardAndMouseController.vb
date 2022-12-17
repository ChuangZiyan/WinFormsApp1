Public Class KeyboardAndMouseController



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

    Public Sub kbHook_KeyDown(ByVal Key As System.Windows.Forms.Keys) Handles kbHook.KeyDown
        'Debug.WriteLine(Key.ToString)
        Select Case Key.ToString
            Case "LControlKey"
                LControlKey_KeyDown = True
            Case "LMenu"
                LMenu_KeyDown = True
        End Select

        If LControlKey_KeyDown And LMenu_KeyDown Then
            Form1.Cursor_X_Position_TextBox.Text = Form1.Cursor_X_Position_Label.Text
            Form1.Cursor_Y_Position_TextBox.Text = Form1.Cursor_Y_Position_Label.Text
        End If


    End Sub
    Public Sub kbHook_KeyUp(ByVal Key As System.Windows.Forms.Keys) Handles kbHook.KeyUp
        'Debug.WriteLine(Key)
        Select Case Key.ToString
            Case "LControlKey"
                LControlKey_KeyDown = False
            Case "LMenu"
                LMenu_KeyDown = False
        End Select
    End Sub
End Class
