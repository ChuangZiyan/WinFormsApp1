<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScriptEditor_Form
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.script_editor_richtextbox = New System.Windows.Forms.RichTextBox()
        Me.run_script_btn = New System.Windows.Forms.Button()
        Me.script_output_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.show_help_btn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'script_editor_richtextbox
        '
        Me.script_editor_richtextbox.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.script_editor_richtextbox.Location = New System.Drawing.Point(12, 12)
        Me.script_editor_richtextbox.Name = "script_editor_richtextbox"
        Me.script_editor_richtextbox.Size = New System.Drawing.Size(591, 486)
        Me.script_editor_richtextbox.TabIndex = 0
        Me.script_editor_richtextbox.Text = ""
        '
        'run_script_btn
        '
        Me.run_script_btn.Location = New System.Drawing.Point(12, 504)
        Me.run_script_btn.Name = "run_script_btn"
        Me.run_script_btn.Size = New System.Drawing.Size(94, 29)
        Me.run_script_btn.TabIndex = 1
        Me.run_script_btn.Text = "Run"
        Me.run_script_btn.UseVisualStyleBackColor = True
        '
        'script_output_RichTextBox
        '
        Me.script_output_RichTextBox.Font = New System.Drawing.Font("Microsoft JhengHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.script_output_RichTextBox.Location = New System.Drawing.Point(609, 12)
        Me.script_output_RichTextBox.Name = "script_output_RichTextBox"
        Me.script_output_RichTextBox.Size = New System.Drawing.Size(640, 486)
        Me.script_output_RichTextBox.TabIndex = 2
        Me.script_output_RichTextBox.Text = ""
        '
        'show_help_btn
        '
        Me.show_help_btn.Location = New System.Drawing.Point(609, 506)
        Me.show_help_btn.Name = "show_help_btn"
        Me.show_help_btn.Size = New System.Drawing.Size(94, 29)
        Me.show_help_btn.TabIndex = 3
        Me.show_help_btn.Text = "Help"
        Me.show_help_btn.UseVisualStyleBackColor = True
        '
        'ScriptEditor_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1261, 547)
        Me.Controls.Add(Me.show_help_btn)
        Me.Controls.Add(Me.script_output_RichTextBox)
        Me.Controls.Add(Me.run_script_btn)
        Me.Controls.Add(Me.script_editor_richtextbox)
        Me.Name = "ScriptEditor_Form"
        Me.Text = "Script Editor"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents script_editor_richtextbox As RichTextBox
    Friend WithEvents run_script_btn As Button
    Friend WithEvents script_output_RichTextBox As RichTextBox
    Friend WithEvents show_help_btn As Button
End Class
