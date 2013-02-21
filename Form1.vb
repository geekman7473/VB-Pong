Public Class Form1
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function GetAsyncKeyState(ByVal vkey As System.Windows.Forms.Keys) As Short
    End Function 'dont worry about this, this is for key state returning
    Dim pucks As System.Drawing.Graphics
    Dim testPuck As puck
    Dim testpaddle As paddle
    Dim sides(3) As Rectangle
    'Dim allPucks(10) As puck
    Private Structure velocity
        Private dX, dY As Double
        Public speed As Long
        'TODO: REWORK VELOCITY
        Sub New(ByVal _dx As Double, ByVal _dy As Double, ByVal _speed As Long)
            If _dx > _dy Then
                dX = _dx / Math.Abs(_dx)
                dY = _dy / Math.Abs(_dx)
            ElseIf _dy > _dx Then
                dX = _dx / Math.Abs(_dy)
                dY = _dy / Math.Abs(_dy)
            Else
                dX = _dx / Math.Abs(_dx)
                dY = _dy / Math.Abs(_dx)
            End If
            speed = _speed
        End Sub
        Public Function inverse()
            Return New velocity(-dX, -dY, speed)
        End Function
        Public Function inverseX()
            Return New velocity(-dX, dY, speed)
        End Function
        Public Function inverseY()
            Return New velocity(dX, -dY, speed)
        End Function
        Public Function fromAngle(ByVal angle As Double, Optional ByVal _speed As Long = 1)
            Return New velocity(Math.Cos(angle) * _speed, Math.Sin(angle) * _speed, _speed)
        End Function
        Public Function getdX()
            Return dX
        End Function
        Public Function getdY()
            Return dY
        End Function
        'Public Sub setdX(ByVal num As Double)
        '    dX = num
        'End Sub

    End Structure
    Private Structure puck
        Public length As Integer
        Public color As Brush
        Public location As Point
        Public velocity As velocity
        Private puckAsRect As Rectangle
        Private graphics As Graphics
        Public Sub updatePos(ByVal ticksPerSec As Integer)
            location.X = location.X + (velocity.getdX * velocity.speed / ticksPerSec)
            location.Y = location.Y + (velocity.getdY * velocity.speed / ticksPerSec)
            'MsgBox(velocity.getdX / ticksPerSec)
        End Sub
        Public Sub drawPuck()
            Form1.pucks.FillRectangle(color, location.X, location.Y, length, length)
        End Sub
        Public Sub Collisiondirection(ByVal wallNum As Short) 'handles collisions with walls
            'MsgBox("collision")
            Select Case wallNum
                Case 0
                    velocity = velocity.inverseX
                Case 1
                    velocity = velocity.inverseY
                Case 2
                    velocity = velocity.inverseX
                Case 3
                    velocity = velocity.inverseY

            End Select
        End Sub
        Public Sub CollisionDirection(ByVal _paddle As paddle)
            'Form1.Timer1.Enabled = False
            Dim tempDX, tempDY As Double
            Dim tempSPEED As Long
            tempDX = location.X - (_paddle.defRect.X + _paddle.defRect.Width / 2)
            tempDY = location.Y - (_paddle.defRect.Y + _paddle.defRect.Height / 2)
            velocity = New velocity(tempDX, tempDY, tempSPEED)
            'MsgBox(velocity.getdX & " " & velocity.getdY & " " & velocity.speed)
            'Form1.Timer1.Enabled = True
        End Sub
        Public Sub initPuck(ByVal Color_ As Brush, ByVal location_ As Point, ByVal length_ As Integer, ByVal direction_ As Double, ByVal speed_ As Double)
            color = Color_
            location = location_
            length = length_
            velocity = velocity.fromAngle(direction_, speed_)
            drawPuck()
        End Sub
        Public Function isColliding(ByVal _rectangle As Rectangle)
            If _rectangle.IntersectsWith(New Rectangle(location.X, location.Y, length, length)) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function isColliding(ByVal _paddle As paddle)
            If _paddle.defRect.IntersectsWith(New Rectangle(location.X, location.Y, length, length)) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function isColliding(ByVal _puck As puck)
            If New Rectangle(_puck.location.X, _puck.location.Y, _puck.length, _puck.length).IntersectsWith(New Rectangle(location.X, location.Y, length, length)) Then
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
        Public Function isColliding(ByVal _rectangle As Rectangle)
            If _rectangle.IntersectsWith(New Rectangle(defRect.X, defRect.Y, defRect.Width, defRect.Height)) Then
                Return True
            Else
                Return False
            End If
        End Function
    End Structure
    'Private Sub New()
    'Dim oldForm As Form
    'oldForm = Me
    'This call is required by the designer.
    'InitializeComponent()
    'Me.SetStyle(ControlStyles.DoubleBuffer, True)
    'Me.SetStyle(ControlStyles.UserPaint, True)
    'Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
    'Me.UpdateStyles()
    'Form1 = oldForm
    ' Add any initialization after the InitializeComponent() call.

    'End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pucks = Me.CreateGraphics
        TextBox3.Text = Me.Width
        TextBox4.Text = Me.Height
        Me.DoubleBuffered = True
        Timer1.Enabled = False
        sides(0).X = Me.Width - 19
        sides(0).Y = 0
        sides(0).Width = 10
        sides(0).Height = Me.Height
        sides(1).X = 0
        sides(1).Y = Me.Height - 40
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
        'Dim buffer As Bitmap
        'Dim backBuffer As Graphics

        ticksPerSec = 1000 / Timer1.Interval
        If GetAsyncKeyState(Keys.Up) Then
            If Not testpaddle.isColliding(sides(3)) Then
                testpaddle.movePaddle(0, -(testpaddle.speed / ticksPerSec))
            Else
                ' MsgBox("IS colliding")
            End If
        End If
        If GetAsyncKeyState(Keys.Down) Then
            If Not testpaddle.isColliding(sides(1)) Then
                testpaddle.movePaddle(0, (testpaddle.speed / ticksPerSec))
            Else
                ' MsgBox("IS colliding")
            End If
        End If
        'pucks.Clear(Me.BackColor)
        For i = 0 To 3
            If testPuck.isColliding(sides(i)) Then testPuck.Collisiondirection(i)
        Next
        If testPuck.isColliding(testpaddle) Then testPuck.Collisiondirection(testpaddle)
        pucks.Clear(Me.BackColor)

        'pucks = Me.CreateGraphics
        'buffer = New Bitmap(MyBase.Width, MyBase.Height)

        testPuck.updatePos(ticksPerSec)
        testPuck.drawPuck()
        testpaddle.drawPaddle()

        'pucks = Graphics.FromImage(buffer)
        'backBuffer = Me.CreateGraphics
        'backBuffer.DrawImage(buffer, 0, 0, Me.Width, Me.Height)
    End Sub
    Private Sub Form1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        pucks = Me.CreateGraphics
        TextBox3.Text = Me.Width
        TextBox4.Text = Me.Height
        sides(0).X = Me.Width
        sides(0).Y = 0
        sides(0).Width = 10
        sides(0).Height = Me.Height
        sides(1).X = 0
        sides(1).Y = Me.Height - 40
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
        'pucks.Clear(Me.BackColor)
        'For i = 0 To 3
        'pucks.FillRectangle(Brushes.Black, sides(i))
        'Next

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        testPuck.initPuck(Brushes.Black, New Point(500, 500), 20, 45, 400)
        testpaddle.initPaddle(70, 70, 40, 100, Brushes.Black, 200)
        Timer1.Enabled = True
        'pucks.DrawLine(Pens.Black, 50, 50, 150, 50)
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Timer1.Interval = Val(TextBox1.Text)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim test As Graphics
        test = Me.CreateGraphics
        test.FillRectangle(Brushes.Brown, CInt(Val(TextBox1.Text)), CInt(Val(TextBox2.Text)), 3, 3)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        MsgBox(Math.Cosh(Val(TextBox5.Text) / CInt(testPuck.length / 2)))
    End Sub
End Class
