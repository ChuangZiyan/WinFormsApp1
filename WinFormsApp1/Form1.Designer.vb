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
        Me.devtype_GroupBox = New System.Windows.Forms.GroupBox()
        Me.ipadair_RadioButton = New System.Windows.Forms.RadioButton()
        Me.i12pro_RadioButton = New System.Windows.Forms.RadioButton()
        Me.pixel5_RadioButton = New System.Windows.Forms.RadioButton()
        Me.pc_RadioButton = New System.Windows.Forms.RadioButton()
        Me.cursor_clickl_btn = New System.Windows.Forms.Button()
        Me.cursor_x_TextBox = New System.Windows.Forms.TextBox()
        Me.cursor_y_TextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.hide_btn = New System.Windows.Forms.Button()
        Me.show_btn = New System.Windows.Forms.Button()
        Me.chromedriver_ListBox = New System.Windows.Forms.ListBox()
        Me.crawl_post_btn = New System.Windows.Forms.Button()
        Me.devtype_GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'write_a_post_btn
        '
        Me.write_a_post_btn.Location = New System.Drawing.Point(178, 528)
        Me.write_a_post_btn.Name = "write_a_post_btn"
        Me.write_a_post_btn.Size = New System.Drawing.Size(100, 60)
        Me.write_a_post_btn.TabIndex = 0
        Me.write_a_post_btn.Text = "Write a post"
        Me.write_a_post_btn.UseVisualStyleBackColor = True
        '
        'content_RichTextBox
        '
        Me.content_RichTextBox.Location = New System.Drawing.Point(178, 259)
        Me.content_RichTextBox.Name = "content_RichTextBox"
        Me.content_RichTextBox.Size = New System.Drawing.Size(341, 263)
        Me.content_RichTextBox.TabIndex = 1
        Me.content_RichTextBox.Text = ""
        '
        'invoke_chrome_btn
        '
        Me.invoke_chrome_btn.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.invoke_chrome_btn.Location = New System.Drawing.Point(12, 462)
        Me.invoke_chrome_btn.Name = "invoke_chrome_btn"
        Me.invoke_chrome_btn.Size = New System.Drawing.Size(160, 60)
        Me.invoke_chrome_btn.TabIndex = 2
        Me.invoke_chrome_btn.Text = "Open Chrome A"
        Me.invoke_chrome_btn.UseVisualStyleBackColor = False
        '
        'driver_close_bnt
        '
        Me.driver_close_bnt.BackColor = System.Drawing.Color.LightCoral
        Me.driver_close_bnt.Location = New System.Drawing.Point(12, 528)
        Me.driver_close_bnt.Name = "driver_close_bnt"
        Me.driver_close_bnt.Size = New System.Drawing.Size(160, 60)
        Me.driver_close_bnt.TabIndex = 3
        Me.driver_close_bnt.Text = "Close Chrome A"
        Me.driver_close_bnt.UseVisualStyleBackColor = False
        '
        'reply_comment_bnt
        '
        Me.reply_comment_bnt.Location = New System.Drawing.Point(284, 528)
        Me.reply_comment_bnt.Name = "reply_comment_bnt"
        Me.reply_comment_bnt.Size = New System.Drawing.Size(108, 60)
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
        Me.Group_ListView.Size = New System.Drawing.Size(449, 447)
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
        Me.replace_str_btn.Location = New System.Drawing.Point(1085, 24)
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
        Me.compare_str_textbox.Size = New System.Drawing.Size(180, 27)
        Me.compare_str_textbox.TabIndex = 13
        '
        'replace_str_textbox
        '
        Me.replace_str_textbox.Location = New System.Drawing.Point(1190, 24)
        Me.replace_str_textbox.Name = "replace_str_textbox"
        Me.replace_str_textbox.Size = New System.Drawing.Size(158, 27)
        Me.replace_str_textbox.TabIndex = 14
        '
        'img_CheckedListBox
        '
        Me.img_CheckedListBox.FormattingEnabled = True
        Me.img_CheckedListBox.HorizontalScrollbar = True
        Me.img_CheckedListBox.Location = New System.Drawing.Point(525, 129)
        Me.img_CheckedListBox.Name = "img_CheckedListBox"
        Me.img_CheckedListBox.Size = New System.Drawing.Size(368, 400)
        Me.img_CheckedListBox.TabIndex = 15
        '
        'devtype_GroupBox
        '
        Me.devtype_GroupBox.Controls.Add(Me.ipadair_RadioButton)
        Me.devtype_GroupBox.Controls.Add(Me.i12pro_RadioButton)
        Me.devtype_GroupBox.Controls.Add(Me.pixel5_RadioButton)
        Me.devtype_GroupBox.Controls.Add(Me.pc_RadioButton)
        Me.devtype_GroupBox.Location = New System.Drawing.Point(12, 306)
        Me.devtype_GroupBox.Name = "devtype_GroupBox"
        Me.devtype_GroupBox.Size = New System.Drawing.Size(160, 150)
        Me.devtype_GroupBox.TabIndex = 16
        Me.devtype_GroupBox.TabStop = False
        Me.devtype_GroupBox.Text = "Device Type"
        '
        'ipadair_RadioButton
        '
        Me.ipadair_RadioButton.AutoSize = True
        Me.ipadair_RadioButton.Location = New System.Drawing.Point(6, 113)
        Me.ipadair_RadioButton.Name = "ipadair_RadioButton"
        Me.ipadair_RadioButton.Size = New System.Drawing.Size(85, 23)
        Me.ipadair_RadioButton.TabIndex = 3
        Me.ipadair_RadioButton.TabStop = True
        Me.ipadair_RadioButton.Text = "iPad Air"
        Me.ipadair_RadioButton.UseVisualStyleBackColor = True
        '
        'i12pro_RadioButton
        '
        Me.i12pro_RadioButton.AutoSize = True
        Me.i12pro_RadioButton.Location = New System.Drawing.Point(6, 84)
        Me.i12pro_RadioButton.Name = "i12pro_RadioButton"
        Me.i12pro_RadioButton.Size = New System.Drawing.Size(128, 23)
        Me.i12pro_RadioButton.TabIndex = 2
        Me.i12pro_RadioButton.TabStop = True
        Me.i12pro_RadioButton.Text = "iPhone 12 Pro"
        Me.i12pro_RadioButton.UseVisualStyleBackColor = True
        '
        'pixel5_RadioButton
        '
        Me.pixel5_RadioButton.AutoSize = True
        Me.pixel5_RadioButton.Location = New System.Drawing.Point(6, 55)
        Me.pixel5_RadioButton.Name = "pixel5_RadioButton"
        Me.pixel5_RadioButton.Size = New System.Drawing.Size(75, 23)
        Me.pixel5_RadioButton.TabIndex = 1
        Me.pixel5_RadioButton.TabStop = True
        Me.pixel5_RadioButton.Text = "Pixel 5"
        Me.pixel5_RadioButton.UseVisualStyleBackColor = True
        '
        'pc_RadioButton
        '
        Me.pc_RadioButton.AutoSize = True
        Me.pc_RadioButton.Checked = True
        Me.pc_RadioButton.Location = New System.Drawing.Point(6, 26)
        Me.pc_RadioButton.Name = "pc_RadioButton"
        Me.pc_RadioButton.Size = New System.Drawing.Size(49, 23)
        Me.pc_RadioButton.TabIndex = 0
        Me.pc_RadioButton.TabStop = True
        Me.pc_RadioButton.Text = "PC"
        Me.pc_RadioButton.UseVisualStyleBackColor = True
        '
        'cursor_clickl_btn
        '
        Me.cursor_clickl_btn.Location = New System.Drawing.Point(689, 73)
        Me.cursor_clickl_btn.Name = "cursor_clickl_btn"
        Me.cursor_clickl_btn.Size = New System.Drawing.Size(102, 32)
        Me.cursor_clickl_btn.TabIndex = 17
        Me.cursor_clickl_btn.Text = "Cursor click"
        Me.cursor_clickl_btn.UseVisualStyleBackColor = True
        '
        'cursor_x_TextBox
        '
        Me.cursor_x_TextBox.Location = New System.Drawing.Point(553, 77)
        Me.cursor_x_TextBox.Name = "cursor_x_TextBox"
        Me.cursor_x_TextBox.Size = New System.Drawing.Size(43, 27)
        Me.cursor_x_TextBox.TabIndex = 18
        '
        'cursor_y_TextBox
        '
        Me.cursor_y_TextBox.Location = New System.Drawing.Point(629, 77)
        Me.cursor_y_TextBox.Name = "cursor_y_TextBox"
        Me.cursor_y_TextBox.Size = New System.Drawing.Size(45, 27)
        Me.cursor_y_TextBox.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(528, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 19)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "X:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(602, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 19)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Y:"
        '
        'hide_btn
        '
        Me.hide_btn.Location = New System.Drawing.Point(12, 129)
        Me.hide_btn.Name = "hide_btn"
        Me.hide_btn.Size = New System.Drawing.Size(77, 29)
        Me.hide_btn.TabIndex = 22
        Me.hide_btn.Text = "Hide"
        Me.hide_btn.UseVisualStyleBackColor = True
        '
        'show_btn
        '
        Me.show_btn.Location = New System.Drawing.Point(95, 129)
        Me.show_btn.Name = "show_btn"
        Me.show_btn.Size = New System.Drawing.Size(77, 29)
        Me.show_btn.TabIndex = 23
        Me.show_btn.Text = "Show"
        Me.show_btn.UseVisualStyleBackColor = True
        '
        'chromedriver_ListBox
        '
        Me.chromedriver_ListBox.FormattingEnabled = True
        Me.chromedriver_ListBox.HorizontalScrollbar = True
        Me.chromedriver_ListBox.ItemHeight = 19
        Me.chromedriver_ListBox.Location = New System.Drawing.Point(178, 129)
        Me.chromedriver_ListBox.Name = "chromedriver_ListBox"
        Me.chromedriver_ListBox.Size = New System.Drawing.Size(341, 118)
        Me.chromedriver_ListBox.TabIndex = 24
        '
        'crawl_post_btn
        '
        Me.crawl_post_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.crawl_post_btn.Location = New System.Drawing.Point(398, 528)
        Me.crawl_post_btn.Name = "crawl_post_btn"
        Me.crawl_post_btn.Size = New System.Drawing.Size(121, 60)
        Me.crawl_post_btn.TabIndex = 25
        Me.crawl_post_btn.Text = "Crawl post"
        Me.crawl_post_btn.UseVisualStyleBackColor = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1354, 596)
        Me.Controls.Add(Me.crawl_post_btn)
        Me.Controls.Add(Me.chromedriver_ListBox)
        Me.Controls.Add(Me.show_btn)
        Me.Controls.Add(Me.hide_btn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cursor_y_TextBox)
        Me.Controls.Add(Me.cursor_x_TextBox)
        Me.Controls.Add(Me.cursor_clickl_btn)
        Me.Controls.Add(Me.devtype_GroupBox)
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
        Me.devtype_GroupBox.ResumeLayout(False)
        Me.devtype_GroupBox.PerformLayout()
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
    Friend WithEvents devtype_GroupBox As GroupBox
    Friend WithEvents pc_RadioButton As RadioButton
    Friend WithEvents i12pro_RadioButton As RadioButton
    Friend WithEvents pixel5_RadioButton As RadioButton
    Friend WithEvents ipadair_RadioButton As RadioButton
    Friend WithEvents cursor_clickl_btn As Button
    Friend WithEvents cursor_x_TextBox As TextBox
    Friend WithEvents cursor_y_TextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cursor_position_lbl As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents hide_btn As Button
    Friend WithEvents show_btn As Button
    Friend WithEvents chromedriver_ListBox As ListBox
    Friend WithEvents crawl_post_btn As Button
End Class
