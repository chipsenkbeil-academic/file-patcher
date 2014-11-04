Public Class frmMain

#Region "Mouse Events for Button"
    Private Sub btnButton_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnButton.MouseDown
        If chkDown.Checked = False Then Exit Sub
        'This occurs when the mouse is currently being pressed on the button.
        Dim sMessage As String = "You are currently holding your mouse down on " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub

    Private Sub btnButton_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnButton.MouseEnter
        If chkEnter.Checked = False Then Exit Sub
        'This occurs when the mouse has just entered the button's area.
        Dim sMessage As String = "Your mouse has just entered " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub

    Private Sub btnButton_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnButton.MouseHover
        If chkHover.Checked = False Then Exit Sub
        'This occurs when the mouse is currently hovering over the button.
        Dim sMessage As String = "Your mouse is hovering over " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub

    Private Sub btnButton_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnButton.MouseLeave
        If chkLeave.Checked = False Then Exit Sub
        'This occurs when the mouse has just left the button's area.
        Dim sMessage As String = "Your mouse has just left " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub

    Private Sub btnButton_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnButton.MouseMove
        If chkMove.Checked = False Then Exit Sub
        'This occurs when the mouse is currently moving on the button.
        Dim sMessage As String = "Your mouse has just moved on " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub

    Private Sub btnButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnButton.MouseUp
        If chkUp.Checked = False Then Exit Sub
        'This occurs when the mouse is currently being released on the button.
        Dim sMessage As String = "You are currently releasing your mouse from " & btnButton.Name & "."
        Me.Text = sMessage
    End Sub
#End Region

End Class
