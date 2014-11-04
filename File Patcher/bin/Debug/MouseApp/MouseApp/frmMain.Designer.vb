<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnButton = New System.Windows.Forms.Button
        Me.SplitContainer = New System.Windows.Forms.SplitContainer
        Me.chkDown = New System.Windows.Forms.CheckBox
        Me.chkUp = New System.Windows.Forms.CheckBox
        Me.chkEnter = New System.Windows.Forms.CheckBox
        Me.chkLeave = New System.Windows.Forms.CheckBox
        Me.chkHover = New System.Windows.Forms.CheckBox
        Me.chkMove = New System.Windows.Forms.CheckBox
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnButton
        '
        Me.btnButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnButton.Location = New System.Drawing.Point(0, 0)
        Me.btnButton.Name = "btnButton"
        Me.btnButton.Size = New System.Drawing.Size(551, 272)
        Me.btnButton.TabIndex = 0
        Me.btnButton.Text = "Button Displaying Mouse Events: Look at Window Caption"
        Me.btnButton.UseVisualStyleBackColor = True
        '
        'SplitContainer
        '
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer.Name = "SplitContainer"
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.btnButton)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.chkMove)
        Me.SplitContainer.Panel2.Controls.Add(Me.chkDown)
        Me.SplitContainer.Panel2.Controls.Add(Me.chkHover)
        Me.SplitContainer.Panel2.Controls.Add(Me.chkUp)
        Me.SplitContainer.Panel2.Controls.Add(Me.chkLeave)
        Me.SplitContainer.Panel2.Controls.Add(Me.chkEnter)
        Me.SplitContainer.Size = New System.Drawing.Size(702, 272)
        Me.SplitContainer.SplitterDistance = 551
        Me.SplitContainer.TabIndex = 1
        '
        'chkDown
        '
        Me.chkDown.AutoSize = True
        Me.chkDown.Checked = True
        Me.chkDown.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDown.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkDown.Location = New System.Drawing.Point(0, 68)
        Me.chkDown.Name = "chkDown"
        Me.chkDown.Size = New System.Drawing.Size(147, 17)
        Me.chkDown.TabIndex = 1
        Me.chkDown.Text = "Register Mouse Down"
        Me.chkDown.UseVisualStyleBackColor = True
        '
        'chkUp
        '
        Me.chkUp.AutoSize = True
        Me.chkUp.Checked = True
        Me.chkUp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUp.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkUp.Location = New System.Drawing.Point(0, 34)
        Me.chkUp.Name = "chkUp"
        Me.chkUp.Size = New System.Drawing.Size(147, 17)
        Me.chkUp.TabIndex = 2
        Me.chkUp.Text = "Register Mouse Up"
        Me.chkUp.UseVisualStyleBackColor = True
        '
        'chkEnter
        '
        Me.chkEnter.AutoSize = True
        Me.chkEnter.Checked = True
        Me.chkEnter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEnter.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkEnter.Location = New System.Drawing.Point(0, 0)
        Me.chkEnter.Name = "chkEnter"
        Me.chkEnter.Size = New System.Drawing.Size(147, 17)
        Me.chkEnter.TabIndex = 3
        Me.chkEnter.Text = "Register Mouse Enter"
        Me.chkEnter.UseVisualStyleBackColor = True
        '
        'chkLeave
        '
        Me.chkLeave.AutoSize = True
        Me.chkLeave.Checked = True
        Me.chkLeave.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLeave.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkLeave.Location = New System.Drawing.Point(0, 17)
        Me.chkLeave.Name = "chkLeave"
        Me.chkLeave.Size = New System.Drawing.Size(147, 17)
        Me.chkLeave.TabIndex = 4
        Me.chkLeave.Text = "Register Mouse Leave"
        Me.chkLeave.UseVisualStyleBackColor = True
        '
        'chkHover
        '
        Me.chkHover.AutoSize = True
        Me.chkHover.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkHover.Location = New System.Drawing.Point(0, 51)
        Me.chkHover.Name = "chkHover"
        Me.chkHover.Size = New System.Drawing.Size(147, 17)
        Me.chkHover.TabIndex = 5
        Me.chkHover.Text = "Register Mouse Hover"
        Me.chkHover.UseVisualStyleBackColor = True
        '
        'chkMove
        '
        Me.chkMove.AutoSize = True
        Me.chkMove.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkMove.Location = New System.Drawing.Point(0, 85)
        Me.chkMove.Name = "chkMove"
        Me.chkMove.Size = New System.Drawing.Size(147, 17)
        Me.chkMove.TabIndex = 6
        Me.chkMove.Text = "Register Mouse Move"
        Me.chkMove.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 272)
        Me.Controls.Add(Me.SplitContainer)
        Me.Name = "frmMain"
        Me.Text = "Mouse Application by smchronos"
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        Me.SplitContainer.Panel2.PerformLayout()
        Me.SplitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnButton As System.Windows.Forms.Button
    Friend WithEvents SplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents chkMove As System.Windows.Forms.CheckBox
    Friend WithEvents chkDown As System.Windows.Forms.CheckBox
    Friend WithEvents chkHover As System.Windows.Forms.CheckBox
    Friend WithEvents chkUp As System.Windows.Forms.CheckBox
    Friend WithEvents chkLeave As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnter As System.Windows.Forms.CheckBox

End Class
