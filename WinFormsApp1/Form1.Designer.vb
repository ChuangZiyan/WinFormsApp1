<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.write_a_post_btn = New System.Windows.Forms.Button()
        Me.content_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.invoke_chrome_btn = New System.Windows.Forms.Button()
        Me.driver_close_bnt = New System.Windows.Forms.Button()
        Me.reply_comment_bnt = New System.Windows.Forms.Button()
        Me.target_url_TextBox = New System.Windows.Forms.TextBox()
        Me.url_lbl = New System.Windows.Forms.Label()
        Me.get_groups_btn = New System.Windows.Forms.Button()
        Me.Group_ListView = New System.Windows.Forms.ListView()
        Me.get_groups_from_m = New System.Windows.Forms.Button()
        Me.curr_url_lbl = New System.Windows.Forms.Label()
        Me.curr_url_TextBox = New System.Windows.Forms.TextBox()
        Me.refresh_url_timer = New System.Windows.Forms.Timer(Me.components)
        Me.replace_str_btn = New System.Windows.Forms.Button()
        Me.compare_str_textbox = New System.Windows.Forms.TextBox()
        Me.replace_str_textbox = New System.Windows.Forms.TextBox()
        Me.img_CheckedListBox = New System.Windows.Forms.CheckedListBox()
        Me.SuspendLayout()
        '
        'write_a_post_btn
        '
        Me.write_a_post_btn.Location = New System.Drawing.Point(178, 528)
        Me.write_a_post_btn.Name = "write_a_post_btn"
        Me.write_a_post_btn.Size = New System.Drawing.Size(160, 60)
        Me.write_a_post_btn.TabIndex = 0
        Me.write_a_post_btn.Text = "Write a post"
        Me.write_a_post_btn.UseVisualStyleBackColor = True
        '
        'content_RichTextBox
        '
        Me.content_RichTextBox.Location = New System.Drawing.Point(178, 129)
        Me.content_RichTextBox.Name = "content_RichTextBox"
        Me.content_RichTextBox.Size = New System.Drawing.Size(341, 393)
        Me.content_RichTextBox.TabIndex = 1
        Me.content_RichTextBox.Text = ""
        '
        'invoke_chrome_btn
        '
        Me.invoke_chrome_btn.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.invoke_chrome_btn.Location = New System.Drawing.Point(12, 129)
        Me.invoke_chrome_btn.Name = "invoke_chrome_btn"
        Me.invoke_chrome_btn.Size = New System.Drawing.Size(160, 60)
        Me.invoke_chrome_btn.TabIndex = 2
        Me.invoke_chrome_btn.Text = "Open Chrome A"
        Me.invoke_chrome_btn.UseVisualStyleBackColor = False
        '
        'driver_close_bnt
        '
        Me.driver_close_bnt.BackColor = System.Drawing.Color.LightCoral
        Me.driver_close_bnt.Location = New System.Drawing.Point(12, 195)
        Me.driver_close_bnt.Name = "driver_close_bnt"
        Me.driver_close_bnt.Size = New System.Drawing.Size(160, 60)
        Me.driver_close_bnt.TabIndex = 3
        Me.driver_close_bnt.Text = "Close Chrome A"
        Me.driver_close_bnt.UseVisualStyleBackColor = False
        '
        'reply_comment_bnt
        '
        Me.reply_comment_bnt.Location = New System.Drawing.Point(359, 528)
        Me.reply_comment_bnt.Name = "reply_comment_bnt"
        Me.reply_comment_bnt.Size = New System.Drawing.Size(160, 60)
        Me.reply_comment_bnt.TabIndex = 4
        Me.reply_comment_bnt.Text = "Reply comment"
        Me.reply_comment_bnt.UseVisualStyleBackColor = True
        '
        'target_url_TextBox
        '
        Me.target_url_TextBox.Location = New System.Drawing.Point(116, 75)
        Me.target_url_TextBox.Name = "target_url_TextBox"
        Me.target_url_TextBox.Size = New System.Drawing.Size(403, 27)
        Me.target_url_TextBox.TabIndex = 5
        '
        'url_lbl
        '
        Me.url_lbl.AutoSize = True
        Me.url_lbl.Location = New System.Drawing.Point(12, 83)
        Me.url_lbl.Name = "url_lbl"
        Me.url_lbl.Size = New System.Drawing.Size(90, 19)
        Me.url_lbl.TabIndex = 6
        Me.url_lbl.Text = "Target URL:"
        '
        'get_groups_btn
        '
        Me.get_groups_btn.Location = New System.Drawing.Point(899, 531)
        Me.get_groups_btn.Name = "get_groups_btn"
        Me.get_groups_btn.Size = New System.Drawing.Size(120, 60)
        Me.get_groups_btn.TabIndex = 7
        Me.get_groups_btn.Text = "Get groups"
        Me.get_groups_btn.UseVisualStyleBackColor = True
        '
        'Group_ListView
        '
        Me.Group_ListView.Location = New System.Drawing.Point(899, 78)
        Me.Group_ListView.Name = "Group_ListView"
        Me.Group_ListView.Size = New System.Drawing.Size(522, 447)
        Me.Group_ListView.TabIndex = 8
        Me.Group_ListView.UseCompatibleStateImageBehavior = False
        '
        'get_groups_from_m
        '
        Me.get_groups_from_m.Location = New System.Drawing.Point(1025, 531)
        Me.get_groups_from_m.Name = "get_groups_from_m"
        Me.get_groups_from_m.Size = New System.Drawing.Size(126, 60)
        Me.get_groups_from_m.TabIndex = 9
        Me.get_groups_from_m.Text = "Get m groups"
        Me.get_groups_from_m.UseVisualStyleBackColor = True
        '
        'curr_url_lbl
        '
        Me.curr_url_lbl.AutoSize = True
        Me.curr_url_lbl.Location = New System.Drawing.Point(12, 24)
        Me.curr_url_lbl.Name = "curr_url_lbl"
        Me.curr_url_lbl.Size = New System.Drawing.Size(98, 19)
        Me.curr_url_lbl.TabIndex = 10
        Me.curr_url_lbl.Text = "Current URL:"
        '
        'curr_url_TextBox
        '
        Me.curr_url_TextBox.Location = New System.Drawing.Point(116, 21)
        Me.curr_url_TextBox.Name = "curr_url_TextBox"
        Me.curr_url_TextBox.Size = New System.Drawing.Size(403, 27)
        Me.curr_url_TextBox.TabIndex = 11
        '
        'refresh_url_timer
        '
        Me.refresh_url_timer.Interval = 2000
        '
        'replace_str_btn
        '
        Me.replace_str_btn.Location = New System.Drawing.Point(1118, 24)
        Me.replace_str_btn.Name = "replace_str_btn"
        Me.replace_str_btn.Size = New System.Drawing.Size(99, 28)
        Me.replace_str_btn.TabIndex = 12
        Me.replace_str_btn.Text = "Replace to"
        Me.replace_str_btn.UseVisualStyleBackColor = True
        '
        'compare_str_textbox
        '
        Me.compare_str_textbox.Location = New System.Drawing.Point(899, 24)
        Me.compare_str_textbox.Name = "compare_str_textbox"
        Me.compare_str_textbox.Size = New System.Drawing.Size(213, 27)
        Me.compare_str_textbox.TabIndex = 13
        '
        'replace_str_textbox
        '
        Me.replace_str_textbox.Location = New System.Drawing.Point(1223, 24)
        Me.replace_str_textbox.Name = "replace_str_textbox"
        Me.replace_str_textbox.Size = New System.Drawing.Size(198, 27)
        Me.replace_str_textbox.TabIndex = 14
        '
        'img_CheckedListBox
        '
        Me.img_CheckedListBox.FormattingEnabled = True
        Me.img_CheckedListBox.Location = New System.Drawing.Point(525, 129)
        Me.img_CheckedListBox.Name = "img_CheckedListBox"
        Me.img_CheckedListBox.Size = New System.Drawing.Size(368, 400)
        Me.img_CheckedListBox.TabIndex = 15
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1584, 600)
        Me.Controls.Add(Me.img_CheckedListBox)
        Me.Controls.Add(Me.replace_str_textbox)
        Me.Controls.Add(Me.compare_str_textbox)
        Me.Controls.Add(Me.replace_str_btn)
        Me.Controls.Add(Me.curr_url_TextBox)
        Me.Controls.Add(Me.curr_url_lbl)
        Me.Controls.Add(Me.get_groups_from_m)
        Me.Controls.Add(Me.Group_ListView)
        Me.Controls.Add(Me.get_groups_btn)
        Me.Controls.Add(Me.url_lbl)
        Me.Controls.Add(Me.target_url_TextBox)
        Me.Controls.Add(Me.reply_comment_bnt)
        Me.Controls.Add(Me.driver_close_bnt)
        Me.Controls.Add(Me.invoke_chrome_btn)
        Me.Controls.Add(Me.content_RichTextBox)
        Me.Controls.Add(Me.write_a_post_btn)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents write_a_post_btn As Button
    Friend WithEvents content_RichTextBox As RichTextBox
    Friend WithEvents invoke_chrome_btn As Button
    Friend WithEvents driver_close_bnt As Button
    Friend WithEvents reply_comment_bnt As Button
    Friend WithEvents target_url_TextBox As TextBox
    Friend WithEvents url_lbl As Label
    Friend WithEvents get_groups_btn As Button
    Friend WithEvents Group_ListView As ListView
    Friend WithEvents get_groups_from_m As Button
    Friend WithEvents curr_url_lbl As Label
    Friend WithEvents curr_url_TextBox As TextBox
    Friend WithEvents refresh_url_timer As Timer
    Friend WithEvents replace_str_btn As Button
    Friend WithEvents compare_str_textbox As TextBox
    Friend WithEvents replace_str_textbox As TextBox
    Friend WithEvents img_CheckedListBox As CheckedListBox
End Class
