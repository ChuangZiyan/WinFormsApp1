﻿Imports System.IO
Imports System.IO.File
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



    Public Sub Save_Script_Content()
        Dim script_txt = ""
        Dim cmd_arrlist As ArrayList = New ArrayList()
        For Each item As ListViewItem In Form1.script_ListView.Items

            Dim tmp_str = ""
            For i = 1 To item.SubItems.Count - 1
                cmd_arrlist.Add(item.SubItems.Item(i).Text)
            Next
            cmd_arrlist(5) = ""
            tmp_str = String.Join(",", cmd_arrlist.ToArray())
            cmd_arrlist.Clear()
            script_txt += tmp_str & vbCrLf

        Next

        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, script_txt)
        End If
    End Sub

    Public Sub Save_RichBox_Content()
        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, Form1.content_RichTextBox.Text)
        End If
    End Sub


    Public Sub Reveal_Selected_In_File_Explorer()
        Dim Current_selected_Item As String = ""
        Dim Selected_Counter As Integer = 0

        'Get TextFile CheckListBox Selected Item

        If Form1.Text_File_CheckedListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.Text_File_CheckedListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        'Get Image CheckListBox Selected Item
        If Form1.img_CheckedListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.img_CheckedListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        If Form1.TextFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.TextFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        If Form1.ImageFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.ImageFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If



        If Form1.Match_Condition_ListView.SelectedItems.Count > 0 Then
            Current_selected_Item = Form1.Match_Condition_ListView.SelectedItems(0).Text + "%20" + Form1.Match_Condition_ListView.SelectedItems(0).SubItems(1).Text
            Selected_Counter += 1
        End If

        Debug.WriteLine("Seleted Counter : " & Selected_Counter)

        If Selected_Counter = 1 Then
            Debug.WriteLine("Seleted : " + Current_selected_Item)
            If Current_selected_Item.Contains("%20") Then
                Dim paths() = Current_selected_Item.Split("%20")
                Debug.WriteLine(paths(0))
                Debug.WriteLine(paths(1))
                Process.Start("explorer.exe", paths(0))
                Process.Start("explorer.exe", paths(1))
            Else
                If Path.GetExtension(Current_selected_Item) <> "" Then
                    Dim fi As New IO.FileInfo(Current_selected_Item)
                    Current_selected_Item = fi.DirectoryName
                End If
                Process.Start("explorer.exe", Current_selected_Item)
            End If


        ElseIf Selected_Counter = 0 Then
            MsgBox("未選取選取任何目錄")
        Else
            MsgBox("不能選擇多於一個")
            Deselect_All_Item()
        End If

    End Sub

    Private Sub Deselect_All_Item()
        If Form1.Text_File_CheckedListBox.SelectedIndex >= 0 Then
            Form1.Text_File_CheckedListBox.SetSelected(Form1.Text_File_CheckedListBox.SelectedIndex, False)
        End If

        If Form1.img_CheckedListBox.SelectedIndex >= 0 Then
            Form1.img_CheckedListBox.SetSelected(Form1.img_CheckedListBox.SelectedIndex, False)
        End If

        If Form1.TextFolder_ListBox.SelectedIndex >= 0 Then
            Form1.TextFolder_ListBox.SetSelected(Form1.TextFolder_ListBox.SelectedIndex, False)
        End If

        If Form1.ImageFolder_ListBox.SelectedIndex >= 0 Then
            Form1.ImageFolder_ListBox.SetSelected(Form1.ImageFolder_ListBox.SelectedIndex, False)
        End If

        If Form1.Match_Condition_ListView.Items.Count > 0 Then
            Form1.Match_Condition_ListView.SelectedItems.Clear()

        End If


    End Sub


End Module
