Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ' Retrieve the image.
            Dim image1 = New Bitmap("C:\Users\wolf\Desktop\map.png", True)

            PictureBox1.Image = enlarge(image1, 5)

            Dim cpos As New coordinates(0, 0) 'current position

            Dim compass As New List(Of coordinates)
            compass.Add(New coordinates(0, 1))
            compass.Add(New coordinates(-1, 0))
            compass.Add(New coordinates(1, 0))
            compass.Add(New coordinates(0, -1))






            Dim pathway As New List(Of coordinates)

            'FIND STARTING POSITION.
            For x = 0 To image1.Width - 1
                For y = 0 To image1.Height - 1
                    If image1.GetPixel(x, y).ToArgb = Color.Lime.ToArgb Then
                        cpos = New coordinates(x, y)
                        pathway.Add(cpos)
                        GoTo startfound 'exit both for loops
                    End If
                Next
            Next
startfound:
            'TODO: make this a subroutine
            'TODO: get contains method to work


            
            'Dim pathway As New List(Of coordinates)
            Dim forks As New List(Of coordinates)
            Dim nearforks As New List(Of coordinates)
            Dim endingfound As Boolean = False

            Dim gx As Integer = 0
            While gx < 1000
                gx += 1
                'check compass directions
                nearforks.Clear()
                For Each Dir As coordinates In compass
                    Dim eval As coordinates = (add(Dir, cpos))
                    'Not (eval.x < 0 Or eval.y < 0 Or contains2(pathway, eval) Or contains2(forks, eval))
                    If Not (eval.x < 0 Or eval.y < 0 Or contains2(pathway, eval) Or contains2(forks, eval)) Then
                        If image1.GetPixel(eval.x, eval.y).ToArgb = Color.White.ToArgb Then
                            nearforks.Add(eval)
                        ElseIf image1.GetPixel(eval.x, eval.y).ToArgb = Color.Red.ToArgb Then
                            gx = 1000 'End While
                            'THIS NEEDS SOMETHING ADDED, I FORGOT
                        End If
                    End If
                Next

                'clear lists
                'compare lists to make sure you're not walking in circles

                Select Case nearforks.Count
                    Case 0 'dead end
                        cpos = forks(forks.Count - 1) 'TODO: combine these two into a subroutine
                        pathway.Add(cpos)
                        forks.RemoveAt(forks.Count - 1)
                    Case 1 'move forward
                        cpos = nearforks(0)
                        pathway.Add(cpos)
                    Case Else
                        forks.AddRange(nearforks)
                        cpos = forks(forks.Count - 1)
                        pathway.Add(cpos)
                        forks.RemoveAt(forks.Count - 1)
                End Select
            End While

            For Each item1 As coordinates In pathway
                ListBox1.Items.Add(item1.x.ToString & ", " & item1.y.ToString)
                image1.SetPixel(item1.x, item1.y, Color.FromArgb(0, 255, 0))
            Next
            For Each item2 As coordinates In forks
                image1.SetPixel(item2.x, item2.y, Color.FromArgb(255, 0, 0))
            Next


            PictureBox1.Image = enlarge(image1, 5)
            MsgBox("Done!")
            MsgBox(forks.Count)





            '            Dim allforks As New List(Of coordinates)
            '            Dim nearforks As New List(Of coordinates)
            '            For l = 1 To 1000 'to keep the program from running forever
            '                pathway.Add(cpos)
            '                addpathway(cpos)

            '                For Each Dir As coordinates In compass
            '                    If Dir.x + cpos.x < 0 Or Dir.y + cpos.y < 0 Then
            '                        GoTo nextfork
            '                    End If
            '                    Select Case image1.GetPixel(Dir.x + cpos.x, Dir.y + cpos.y).ToArgb
            '                        Case Color.Red.ToArgb
            '                            MsgBox("maze complete!")
            '                            GoTo done
            '                        Case Color.White.ToArgb
            '                            If Not contains2(pathway, New coordinates(Dir.x + cpos.x, Dir.y + cpos.y)) Then 'don't walk in circles
            '                                'forks.Add(New coordinates(Dir.x + cpos.x, Dir.y + cpos.y))
            '                                allforks.Add(New coordinates(Dir.x + cpos.x, Dir.y + cpos.y))
            '                                nearforks.Add(New coordinates(Dir.x + cpos.x, Dir.y + cpos.y))

            '                            End If
            '                    End Select
            'nextfork:
            '                Next
            '                Select Case nearforks.Count
            '                    Case 0
            '                        cpos = forks(forks.Count - 1)
            '                        forks.RemoveAt(forks.Count - 1)
            '                    Case 1
            '                        cpos = nearforks(0) 'progress through maze
            '                    Case Else
            '                        forks.AddRange(nearforks)
            '                        cpos = forks(forks.Count - 1)
            '                        forks.RemoveAt(forks.Count - 1)
            '                End Select
            '            Next
            'Done:
            '            'remove each item of forks contained in allforks
            '            For Each asdf As coordinates In forks
            '                If contains2(allforks, asdf) Then
            '                    allforks.Remove(asdf)

            '                    'TODO: check to see that this is actually removing items
            '                    'TODO: remove ALL instances of any given coordinate (not just the first one)

            '                    'MsgBox("removed item " & asdf.x & ", " & asdf.y)
            '                End If
            '            Next


            '            For Each thingie As coordinates In pathway
            '                image1.SetPixel(thingie.x, thingie.y, Color.FromArgb(0, 255, 0))
            '            Next

            '            For Each thingie As coordinates In allforks
            '                image1.SetPixel(thingie.x, thingie.y, Color.FromArgb(255, 255, 0))
            '                PictureBox1.Image = enlarge(image1, 5)
            '            Next
            '            'TODO: make a loading bar


        Catch ex As ArgumentException
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Public Function add(ByVal a As coordinates, ByVal b As coordinates)
        Return New coordinates(a.x + b.x, a.y + b.y)
    End Function

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
        'ListBox1.Items.Add(cd.x.ToString & ", " & cd.y.ToString)
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