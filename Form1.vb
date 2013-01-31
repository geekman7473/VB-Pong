Public Class Form1
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function GetAsyncKeyState(ByVal vkey As System.Windows.Forms.Keys) As Short
    End Function 'dont worry about this, this is for key state returning
    Dim pucks As System.Drawing.Graphics
    Dim testPuck As puck
    Dim testpaddle As paddle
    Dim sides(3) As Rectangle
    'Dim allPucks(10) As puck
    Private Structure puck
        'Private Dim draw As System.Drawing
        Public radius As Integer
        Public color As Brush
        Public location As Point
        Public pixelsPerSec As Double
        Public direction As Double
        Private velocityX, velocityY As Double 'so i can access the X and Y velocities
        Private puckAsRect As Rectangle
        Public Sub updatePos(ByVal ticksPerSec As Integer)
            location.X = location.X + ((pixelsPerSec / ticksPerSec) * velocityX)
            location.Y = location.Y + ((pixelsPerSec / ticksPerSec) * velocityY)
        End Sub
        Public Sub drawPuck()
            Form1.pucks.FillEllipse(color, location.X, location.Y, radius, radius)
        End Sub
        Public Sub wallCollisionDirection(ByVal wallNum As Short)
            ' MsgBox(direction)
            If ((direction > 0 And direction <= 90) Or (direction > 180 And direction <= 270)) And (wallNum = 0 Or wallNum = 2) Then 'checks quad 1 and 3 against side walls
                '  MsgBox("sidewall" & (direction > 0 And direction <= 90) & (direction > 180 And direction <= 270))
                direction = (direction - 2 * (direction Mod 90)) + 180
            ElseIf ((direction > 90 And direction <= 180) Or ((direction > 270 And direction <= 359) Or direction = 0)) And (wallNum = 1 Or wallNum = 3) Then 'checks quads 2 and 4 against top wall
                direction = (direction - 2 * (direction Mod 90)) + 180
            Else : direction = (direction - 2 * (direction Mod 90))
            End If
            If direction < 0 Then
                direction = 360 - Math.Abs(direction Mod 360)
            Else
                direction = direction Mod 360
            End If
            updateVelocity()
        End Sub
        Private Sub updateVelocity()
            velocityX = Math.Cos(direction * (Math.PI / 180))
            velocityY = Math.Sin(direction * (Math.PI / 180))
            'MsgBox(velocityY)
        End Sub
        Public Sub initPuck(ByVal Color_ As Brush, ByVal location_ As Point, ByVal radius_ As Integer, ByVal direction_ As Double, ByVal pixelsPerSec_ As Double)
            color = Color_
            location = location_
            radius = radius_
            direction = direction_
            pixelsPerSec = pixelsPerSec_
            updateVelocity()
            drawPuck()
            'puckAsRect.X = location.X
            'puckAsRect.Y = 
        End Sub
        Public Function isColliding(ByVal _rectangle As Rectangle)
            If _rectangle.IntersectsWith(New Rectangle(location.X, location.Y, radius * 2, radius * 2)) Then
                Return False
            Else
                Return True
            End If
        End Function
    End Structure
    Private Structure paddle
        Public defRect As Rectangle
        Public color As Brush
        Public speed As Integer
        Public Sub drawPaddle()
            Form1.pucks.FillRectangle(color, defRect)
        End Sub
        Public Sub movePaddle(ByVal distance As Point)
            defRect.X += distance.X
            defRect.Y += distance.Y
        End Sub
        Public Sub movepaddle(ByVal x As Integer, ByVal y As Integer)
            defRect.X += x
            defRect.Y += y
        End Sub
        Public Sub initPaddle(ByVal _rectangle As Rectangle, ByVal _color As Brush, ByVal _velocity As Integer)
            defRect = _rectangle
            color = _color
            speed = _velocity
            drawPaddle()
        End Sub
        Public Sub initPaddle(ByVal _location As Point, ByVal _width As Integer, ByVal _height As Integer, ByVal _color As Brush, ByVal _velocity As Integer)
            defRect.X = _location.X
            defRect.Y = _location.Y
            defRect.Width = _width
            defRect.Height = _height
            color = _color
            speed = _velocity
            drawPaddle()
        End Sub
        Public Sub initPaddle(ByVal xPos As Integer, ByVal yPos As Integer, ByVal _width As Integer, ByVal _height As Integer, ByVal _color As Brush, ByVal _velocity As Integer)
            defRect.X = xPos
            defRect.Y = yPos
            defRect.Width = _width
            defRect.Height = _height
            color = _color
            speed = _velocity
            drawPaddle()
        End Sub
    End Structure
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pucks = Me.CreateGraphics
        Timer1.Enabled = False
        sides(0).X = Me.Width
        sides(0).Y = 0
        sides(0).Width = 10
        sides(0).Height = Me.Height
        sides(1).X = 0
        sides(1).Y = Me.Height - 25
        sides(1).Width = Me.Width
        sides(1).Height = 10
        sides(2).X = -10
        sides(2).Y = 0
        sides(2).Width = 10
        sides(2).Height = Me.Height
        sides(3).X = 0
        sides(3).Y = -10
        sides(3).Width = Me.Width
        sides(3).Height = 10
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim ticksPerSec As Short
        ticksPerSec = 1000 / Timer1.Interval
        If GetAsyncKeyState(Keys.Up) Then
            testpaddle.movePaddle(0, -(testpaddle.speed / ticksPerSec))
        End If
        If GetAsyncKeyState(Keys.Down) Then
            testpaddle.movePaddle(0, (testpaddle.speed / ticksPerSec))
        End If
        pucks.Clear(Me.BackColor)
        For i = 0 To 3
            If testPuck.isColliding(sides(i)) Then testPuck.wallCollisionDirection(i)
        Next
        testPuck.updatePos(ticksPerSec)
        testPuck.drawPuck()
        testpaddle.drawPaddle()

    End Sub
    Private Sub Form1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        pucks = Me.CreateGraphics
        sides(0).X = Me.Width
        sides(0).Y = 0
        sides(0).Width = 10
        sides(0).Height = Me.Height
        sides(1).X = 0
        sides(1).Y = Me.Height - 20
        sides(1).Width = Me.Width
        sides(1).Height = 10
        sides(2).X = -10
        sides(2).Y = 0
        sides(2).Width = 10
        sides(2).Height = Me.Height
        sides(3).X = 0
        sides(3).Y = -10
        sides(3).Width = Me.Width
        sides(3).Height = 10
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        testPuck.initPuck(Brushes.Black, New Point(50, 50), 20, Rnd() * 359, 600)
        testpaddle.initPaddle(70, 70, 40, 100, Brushes.Black, 200)
        Timer1.Enabled = True
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Timer1.Interval = Val(TextBox1.Text)
    End Sub
End Class
