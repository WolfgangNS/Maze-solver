Public Class Form1

    Dim puregreen As Color = Color.FromArgb(0, 255, 0)
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ' Retrieve the image.
            Dim image1 = New Bitmap("C:\Users\wolf\Desktop\map.png", True)

            PictureBox1.Image = enlarge(image1, 5)

            Dim cpos As New coordinates(0, 0) 'current position

            Dim pathway As New List(Of coordinates)
            Dim forks As New List(Of coordinates)


            Dim compass As New List(Of coordinates)
            compass.Add(New coordinates(0, 1))
            compass.Add(New coordinates(-1, 0))
            compass.Add(New coordinates(1, 0))
            compass.Add(New coordinates(0, -1))






            'FIND STARTING POSITION.
            For x = 0 To image1.Width - 1
                For y = 0 To image1.Height - 1
                    If image1.GetPixel(x, y).ToArgb = puregreen.ToArgb Then
                        cpos = New coordinates(x, y)
                        GoTo startfound 'exit both for loops
                    End If
                Next
            Next
startfound:
            'TODO: make this a subroutine


            'pathway.Add(New coordinates(28, 28))
            'MsgBox(contains2(pathway, New coordinates(28, 28)).ToString)
            'MsgBox(contains2(pathway, New coordinates(28, 29)).ToString)

            'MsgBox(pathway.Contains(New coordinates(28, 28)).ToString)
            ''the "contain" method doesn't work for some reason
            '-------TODO: get contains method to work


            Dim tempforks As New List(Of coordinates)
            For l = 1 To 1000
                tempforks.Clear()
                pathway.Add(cpos)
                addpathway(cpos)

                For Each Dir As coordinates In compass
                    If Dir.x + cpos.x < 0 Or Dir.y + cpos.y < 0 Then
                        GoTo nextfork
                    End If
                    Select Case image1.GetPixel(Dir.x + cpos.x, Dir.y + cpos.y).ToArgb
                        Case Color.Red.ToArgb
                            MsgBox("maze complete!")
                            GoTo done
                        Case Color.White.ToArgb
                            If Not contains2(pathway, New coordinates(Dir.x + cpos.x, Dir.y + cpos.y)) Then 'don't walk in circles
                                tempforks.Add(New coordinates(Dir.x + cpos.x, Dir.y + cpos.y))

                            End If
                    End Select
nextfork:
                Next

                If tempforks.Count > 1 Then
                    forks.AddRange(tempforks)
                    cpos = forks(forks.Count - 1)
                    forks.RemoveAt(forks.Count - 1)
                Else
                    cpos = tempforks(0) 'progress through the maze
                End If
            Next

Done:

            For Each thingie As coordinates In pathway
                image1.SetPixel(thingie.x, thingie.y, Color.FromArgb(0, 255, 0))
            Next

            For Each thingie As coordinates In forks
                image1.SetPixel(thingie.x, thingie.y, Color.FromArgb(0, 255, 255))
                PictureBox1.Image = enlarge(image1, 5)
            Next
            'TODO: make a loading bar


        Catch ex As ArgumentException
            MessageBox.Show(ex.ToString)
        End Try
    End Sub


    Public Function enlarge(ByVal asdf As Bitmap, ByVal size As Integer) 'ByVal or ByRef? - IMPORTANT DISTINCTION
        'doubles the size of the image
        Dim ghi As New Bitmap(asdf.Width * size, asdf.Height * size)
        For p = 0 To (ghi.Width - 1)
            For q = 0 To (ghi.Height - 1)
                ghi.SetPixel(p, q, asdf.GetPixel(Math.Floor(p / size), Math.Floor(q / size)))
            Next
        Next
        Return ghi
    End Function

    Public Function contains2(ByVal coordlist As List(Of coordinates), ByVal compareitem As coordinates) 'I really need to think of better names
        For Each thing As coordinates In coordlist
            If thing.x = compareitem.x And thing.y = compareitem.y Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub addpathway(ByVal cd As coordinates)
        ListBox1.Items.Add(cd.x.ToString & ", " & cd.y.ToString)
    End Sub
End Class

Public Class coordinates
    Public Property x As Integer
    Public Property y As Integer
    Public Sub New(ByVal setx As Integer, ByVal sety As Integer, Optional ByVal forknumer As Integer = -1)
        x = setx
        y = sety
    End Sub
End Class


'tools:
'Try
'' Retrieve the image.
'Dim image1 = New Bitmap("C:\Users\wolf\Desktop\map.png", True)
'Dim g As Drawing.Graphics = Drawing.Graphics.FromImage(image1)
'PictureBox1.Image = image1
'Dim x, y As Integer


'image1.GetPixel(x, y)
'image1.SetPixel(x, y, Color.LimeGreen)
'Color.Red.ToArgb
'Dim puregreen As Color = Color.FromArgb(0, 255, 0)
'image1.Width
'image1.Height