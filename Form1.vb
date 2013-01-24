Public Class Form1
    Dim pucks As System.Drawing.Graphics
    Dim testPuck As puck
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pucks = Me.CreateGraphics
    End Sub
    Private Sub Form1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        pucks = Me.CreateGraphics
    End Sub
    Private Structure puck
        'Private Dim draw As System.Drawing
        Public radius As Integer
        Public color As Brush
        Public location As Point
        Public pixelsPerSec As Double
        Public direction As Double
        Public Sub updatePos()
            'MsgBox(location.Y & " " & location.X & " " & pixelsPerSec * Math.Cos(direction) & " " & (pixelsPerSec * Math.Sin(direction)) & " " & Math.Sqrt(Math.Pow(location.X + (pixelsPerSec * Math.Cos(direction)) - location.X, 2) + Math.Pow(location.Y + (pixelsPerSec * Math.Sin(direction)) - location.Y, 2)))
            location.X = location.X + (pixelsPerSec * Math.Cos(direction * (Math.PI / 180)))
            location.Y = location.Y + (pixelsPerSec * Math.Sin(direction * (Math.PI / 180)))
        End Sub
    Public Sub drawPuck()
        Form1.pucks.FillEllipse(color, location.X, location.Y, radius, radius)
    End Sub
    End Structure

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        testPuck.radius = 15
        testPuck.color = Brushes.Black
        testPuck.location = New Point(50, 50)
        testPuck.direction = 0
        testPuck.pixelsPerSec = 5
        testPuck.drawPuck()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        pucks.Clear(Me.BackColor)
        If TextBox1.Text <> "" Then testPuck.direction = TextBox1.Text
        testPuck.updatePos()
        testPuck.drawPuck()
    End Sub
End Class
