Public Class Form1
    Dim pucks As System.Drawing.Graphics
    Dim testPuck As puck
    Private Structure puck
        'Private Dim draw As System.Drawing
        Public radius As Integer
        Public color As Brush
        Public location As Point
        Public pixelsPerSec As Double
        Public direction As Double
        Public Sub updatePos(ByVal ticksPerSec As Integer)
            'MsgBox(location.Y & " " & location.X & " " & pixelsPerSec * Math.Cos(direction) & " " & (pixelsPerSec * Math.Sin(direction)) & " " & Math.Sqrt(Math.Pow(location.X + (pixelsPerSec * Math.Cos(direction)) - location.X, 2) + Math.Pow(location.Y + (pixelsPerSec * Math.Sin(direction)) - location.Y, 2)))
            'MsgBox(location.X & location.Y)
            location.X = location.X + ((pixelsPerSec / ticksPerSec) * Math.Cos(direction * (Math.PI / 180)))
            location.Y = location.Y + ((pixelsPerSec / ticksPerSec) * Math.Sin(direction * (Math.PI / 180)))
            'MsgBox(location.X & location.Y)
        End Sub
        Public Sub drawPuck()
            Form1.pucks.FillEllipse(color, location.X, location.Y, radius, radius)
        End Sub
        Public Sub collisionVector()
            direction = (direction - 2 * (direction Mod 90)) - 180
            If direction < 0 Then
                direction = 360 - Math.Abs(direction Mod 360)
            Else
                direction = direction Mod 360
            End If

        End Sub
    End Structure
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        pucks.Clear(Me.BackColor)
        testPuck.updatePos(1000 / Timer1.Interval)
        testPuck.drawPuck()
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pucks = Me.CreateGraphics
        Timer1.Enabled = False
    End Sub
    Private Sub Form1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        pucks = Me.CreateGraphics
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        testPuck.radius = 15
        testPuck.color = Brushes.Black
        testPuck.location = New Point(50, 50)
        testPuck.direction = 0
        testPuck.pixelsPerSec = 100
        testPuck.drawPuck()
        Timer1.Enabled = True
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        pucks.Clear(Me.BackColor)
        If TextBox1.Text <> "" Then testPuck.direction = TextBox1.Text
        testPuck.updatePos(1)
        testPuck.drawPuck()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        testPuck.collisionVector()
    End Sub
End Class
