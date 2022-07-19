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
        Me.write_a_post_btn = New System.Windows.Forms.Button()
        Me.content_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.invoke_chrome_btn = New System.Windows.Forms.Button()
        Me.driver_close_bnt = New System.Windows.Forms.Button()
        Me.reply_comment_bnt = New System.Windows.Forms.Button()
        Me.url_TextBox = New System.Windows.Forms.TextBox()
        Me.url_lbl = New System.Windows.Forms.Label()
        Me.get_groups_btn = New System.Windows.Forms.Button()
        Me.Group_ListView = New System.Windows.Forms.ListView()
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
        Me.content_RichTextBox.Location = New System.Drawing.Point(178, 63)
        Me.content_RichTextBox.Name = "content_RichTextBox"
        Me.content_RichTextBox.Size = New System.Drawing.Size(341, 459)
        Me.content_RichTextBox.TabIndex = 1
        Me.content_RichTextBox.Text = ""
        '
        'invoke_chrome_btn
        '
        Me.invoke_chrome_btn.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.invoke_chrome_btn.Location = New System.Drawing.Point(12, 63)
        Me.invoke_chrome_btn.Name = "invoke_chrome_btn"
        Me.invoke_chrome_btn.Size = New System.Drawing.Size(160, 60)
        Me.invoke_chrome_btn.TabIndex = 2
        Me.invoke_chrome_btn.Text = "Open Chrome"
        Me.invoke_chrome_btn.UseVisualStyleBackColor = False
        '
        'driver_close_bnt
        '
        Me.driver_close_bnt.BackColor = System.Drawing.Color.LightCoral
        Me.driver_close_bnt.Location = New System.Drawing.Point(12, 129)
        Me.driver_close_bnt.Name = "driver_close_bnt"
        Me.driver_close_bnt.Size = New System.Drawing.Size(160, 60)
        Me.driver_close_bnt.TabIndex = 3
        Me.driver_close_bnt.Text = "Close Chrome"
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
        'url_TextBox
        '
        Me.url_TextBox.Location = New System.Drawing.Point(59, 12)
        Me.url_TextBox.Name = "url_TextBox"
        Me.url_TextBox.Size = New System.Drawing.Size(460, 27)
        Me.url_TextBox.TabIndex = 5
        '
        'url_lbl
        '
        Me.url_lbl.AutoSize = True
        Me.url_lbl.Location = New System.Drawing.Point(12, 20)
        Me.url_lbl.Name = "url_lbl"
        Me.url_lbl.Size = New System.Drawing.Size(41, 19)
        Me.url_lbl.TabIndex = 6
        Me.url_lbl.Text = "URL:"
        '
        'get_groups_btn
        '
        Me.get_groups_btn.Location = New System.Drawing.Point(563, 528)
        Me.get_groups_btn.Name = "get_groups_btn"
        Me.get_groups_btn.Size = New System.Drawing.Size(522, 60)
        Me.get_groups_btn.TabIndex = 7
        Me.get_groups_btn.Text = "Get groups"
        Me.get_groups_btn.UseVisualStyleBackColor = True
        '
        'Group_ListView
        '
        Me.Group_ListView.Location = New System.Drawing.Point(563, 12)
        Me.Group_ListView.Name = "Group_ListView"
        Me.Group_ListView.Size = New System.Drawing.Size(522, 510)
        Me.Group_ListView.TabIndex = 8
        Me.Group_ListView.UseCompatibleStateImageBehavior = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1097, 600)
        Me.Controls.Add(Me.Group_ListView)
        Me.Controls.Add(Me.get_groups_btn)
        Me.Controls.Add(Me.url_lbl)
        Me.Controls.Add(Me.url_TextBox)
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
    Friend WithEvents url_TextBox As TextBox
    Friend WithEvents url_lbl As Label
    Friend WithEvents get_groups_btn As Button
    Friend WithEvents Group_ListView As ListView
End Class
