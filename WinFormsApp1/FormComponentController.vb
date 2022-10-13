Module FormComponentController

    Public Sub Delete_ScriptListView_selected_item()
        For i As Integer = Form1.script_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Form1.script_ListView.Items.RemoveAt(Form1.script_ListView.SelectedIndices(i))
        Next
        Rearrange_scriptlistview_number()
    End Sub

    Public Sub Move_up_ScriptListView_selected_item()
        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = 0 To Form1.script_ListView.SelectedIndices.Count - 1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index > 0 Then
                    If Form1.script_ListView.SelectedIndices.Contains(index - 1) Then
                        Continue For
                    End If
                    Dim temp As ListViewItem = Form1.script_ListView.Items(index)
                    Form1.script_ListView.Items.RemoveAt(index)
                    Form1.script_ListView.Items.Insert(index - 1, temp)
                    Form1.script_ListView.Items(index - 1).Focused = True
                End If
            Next
        End If
        Rearrange_scriptlistview_number()
    End Sub

    Public Sub MoveDown_ScriptListView_selected_item()
        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = Form1.script_ListView.SelectedIndices.Count - 1 To 0 Step -1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index < Form1.script_ListView.Items.Count - 1 Then
                    If Form1.script_ListView.SelectedIndices.Contains(index + 1) Then
                        Continue For
                    End If
                    Dim temp As ListViewItem = Form1.script_ListView.Items(index)
                    Form1.script_ListView.Items.RemoveAt(index)
                    Form1.script_ListView.Items.Insert(index + 1, temp)
                    Form1.script_ListView.Items(index + 1).Focused = True
                End If
            Next
        End If
        Rearrange_scriptlistview_number()
    End Sub

    Public Sub Rearrange_scriptlistview_number()
        Dim index = 1
        For Each item As ListViewItem In Form1.script_ListView.Items
            item.SubItems.Item(0).Text = index.ToString()
            index += 1
        Next

    End Sub




End Module
