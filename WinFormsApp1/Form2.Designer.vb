<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.test_success_log_btn = New System.Windows.Forms.Button()
        Me.test_fail_log_btn = New System.Windows.Forms.Button()
        Me.eventlog_ListView = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'test_success_log_btn
        '
        Me.test_success_log_btn.Location = New System.Drawing.Point(12, 521)
        Me.test_success_log_btn.Name = "test_success_log_btn"
        Me.test_success_log_btn.Size = New System.Drawing.Size(140, 50)
        Me.test_success_log_btn.TabIndex = 3
        Me.test_success_log_btn.Text = "Test Success"
        Me.test_success_log_btn.UseVisualStyleBackColor = True
        '
        'test_fail_log_btn
        '
        Me.test_fail_log_btn.Location = New System.Drawing.Point(199, 521)
        Me.test_fail_log_btn.Name = "test_fail_log_btn"
        Me.test_fail_log_btn.Size = New System.Drawing.Size(154, 50)
        Me.test_fail_log_btn.TabIndex = 4
        Me.test_fail_log_btn.Text = "Test Fail"
        Me.test_fail_log_btn.UseVisualStyleBackColor = True
        '
        'eventlog_ListView
        '
        Me.eventlog_ListView.Location = New System.Drawing.Point(12, 12)
        Me.eventlog_ListView.Name = "eventlog_ListView"
        Me.eventlog_ListView.Size = New System.Drawing.Size(999, 503)
        Me.eventlog_ListView.TabIndex = 5
        Me.eventlog_ListView.UseCompatibleStateImageBehavior = False
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1023, 588)
        Me.Controls.Add(Me.eventlog_ListView)
        Me.Controls.Add(Me.test_fail_log_btn)
        Me.Controls.Add(Me.test_success_log_btn)
        Me.Name = "Form2"
        Me.Text = "Selenium Log"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents test_success_log_btn As Button
    Friend WithEvents test_fail_log_btn As Button
    Friend WithEvents eventlog_ListView As ListView
End Class
