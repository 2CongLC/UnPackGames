'https://github.com/marcussacana/SDR2CM/tree/master
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text


    Class Program
        Private Shared Sub Main(args As String())
            If args Is New String(0) {}
                Console.Write("Super Danganronpa 2 Content Manager" & vbLf & "This tool has created by marcus and is a open source project." & vbLf & vbLf & "Usage:" & vbLf & "SDR2CM.exe In_file_to_extract" & vbLf & "Or:" & vbLf & "SDR2CM.exe In_Folder_to_Repack")
            Else
                For Each arg As String In args
                    If System.IO.File.Exists(arg) Then
                        Try
                            ExtractPak(Tools.ByteArrayToString(System.IO.File.ReadAllBytes(arg)).Split("-"c), System.IO.Path.GetDirectoryName(arg) & "\" & System.IO.Path.GetFileName(arg) & "-out\")
                            Console.Write(vbLf & System.IO.Path.GetFileName(arg) & " Extracted..." & vbLf)
                        Catch
                            Console.Write(vbLf & "Invalid File Format")
                        End Try
                    ElseIf System.IO.Directory.Exists(arg) Then
                        Dim folder As String = arg
                        If arg.EndsWith("\") Then
                            folder = arg.Substring(0, arg.Length - 1)
                        End If
                        RepackPak(folder, System.IO.Path.GetDirectoryName(folder) & "\" & System.IO.Path.GetFileName(folder).Replace("-out", ""))
                    Else
                        Console.Write("Invalid Folder or File, Make sure you write full file/folder path.")
                    End If
                Next
            End If
        End Sub

        Private Shared Sub RepackPak(Directory As String, PakPath As String)
            Dim ExtraHeaderContent As String = ""
            Dim ExtraHeader As Boolean = False
            Dim count As Integer = -1
again:
            count += 1
            Dim files As String() = System.IO.Directory.GetFiles(Directory).OrderBy(Function(f) f).ToArray()
            Dim FOrder As String() = New String(0) {}
            Dim FILE As Integer = 0
            Do While files.Length > FOrder.Length
                If count = 0 AndAlso System.IO.File.Exists((Directory & "\" & "HeaderContent.SDR2CM")) Then
                    Exit Do
                End If
                If FILE >= files.Length Then
                    FILE = 0
                End If
                If System.IO.Path.GetFileNameWithoutExtension(files(FILE)) = FOrder.Length.ToString() Then
                    Dim temp As String() = New String(FOrder.Length) {}
                    FOrder.CopyTo(temp, 0)
                    temp(FOrder.Length) = files(FILE)
                    FOrder = temp
                End If
                FILE += 1
            Loop
            Dim PakHexs As String() = New String(0) {}
            Packget = New File(files.Length - 1) {}
            For i As Integer = 0 To Packget.Length - 1
                If Not (System.IO.Path.GetFileName(files(i)) = "HeaderContent.SDR2CM") Then
                    Packget(i) = New File()
                Else
                    ExtraHeaderContent = System.IO.File.ReadAllText(files(i))
                    System.IO.File.Delete(files(i))
                    ExtraHeader = True
                    GoTo again
                End If
            Next
            files = FOrder
            If ExtraHeader Then
                System.IO.File.WriteAllText(Directory & "\" & "HeaderContent.SDR2CM", ExtraHeaderContent)
            End If
            Dim Header As String = Tools.IntToHex(Packget.Length) & " "
            For i As Integer = 0 To Packget.Length - 1
                PakHexs = AppendFile(PakHexs, Tools.ByteArrayToString(System.IO.File.ReadAllBytes(files(i))).Split("-"c), i)
                Header &= Tools.IntToHex(0) & " "
            Next
            Header &= ExtraHeaderContent
            Dim HeaderSize As Integer = (Header.Replace(" ", "").Length / 2)

            Header = Tools.IntToHex(Packget.Length) & " "
            For i As Integer = 0 To Packget.Length - 1
                Header &= Tools.IntToHex(Packget(i).StartPos + HeaderSize) & " "
            Next
            Header &= ExtraHeaderContent
confirm:
            If Header.EndsWith(" ") Then
                Header = Header.Substring(0, Header.Length - 1)
                GoTo confirm
            End If
            Dim OutFile As String() = New String((Header.Replace(" ", "").Length / 2) + PakHexs.Length - 1) {}
            Dim PakHeader As String() = Header.Split(" "c)
            PakHeader.CopyTo(OutFile, 0)
            PakHexs.CopyTo(OutFile, PakHeader.Length)
            System.IO.File.WriteAllBytes(PakPath, Tools.StringToByteArray(OutFile))
        End Sub

        Private Shared Function AppendFile(Pak As String(), FileToWrite As String(), id As Integer) As String()
            Dim temp As String() = New String(Pak.Length + FileToWrite.Length - 1) {}
            Pak.CopyTo(temp, 0)
            Packget(id).StartPos = Pak.Length
            FileToWrite.CopyTo(temp, Pak.Length)
            Return temp
        End Function


        Public Shared Packget As File()
        Private Shared Sub ExtractPak(pak As String(), OutPath As String)
            Dim TotalFiles As Integer = GetOffset(pak, 0)
            Packget = New File(TotalFiles - 1) {}
            Console.Write("Total files: " & TotalFiles)
            Dim Offset As Integer = 4
            Do While (Offset / 4) <= TotalFiles
                Dim id As Integer = (Offset / 4) - 1
                Packget(id) = New File()
                Packget(id).StartPos = GetOffset(pak, Offset)
                If (Offset + 1) / 4 < TotalFiles Then
                    Packget(id).EndPos = (GetOffset(pak, (Offset + 4)) - 1)
                Else
                    Packget(id).EndPos = (pak.Length - 1)
                End If
                Console.Write(vbLf & "File: " & id & ", Start at: " & Packget(id).StartPos & " and ends at: " & Packget(id).EndPos)
                Offset += 4
            Loop
            Console.Write(vbLf & "File Offset Tree generated, Getting Extra Content...")
            Dim ExtraHeaderContent As String = ""
            If Packget Is New File(0) {} Then
                Return
            Else
                If Packget(0) Is Nothing Then
                    Return
                End If
            End If
            For Offset As Integer = (((TotalFiles + 1) * 4)) To Packget(0).StartPos - 1
                ExtraHeaderContent += pak(Offset) & " "
            Next
            System.IO.Directory.CreateDirectory(OutPath)
            If ExtraHeaderContent <> "" Then
                System.IO.File.WriteAllText(OutPath & "HeaderContent.SDR2CM", ExtraHeaderContent.Substring(0, ExtraHeaderContent.Length - 1))
            End If
            Console.Write(vbLf & "Packget Header generated, Identifying files formats...")
            For id As Integer = 0 To TotalFiles - 1
                Packget(id).Extension = GetFormat(pak, Packget(id).StartPos)
            Next
            Console.Write(vbLf & "All formats identified... Starting Extraction")
            For id As Integer = 0 To TotalFiles - 1
                Dim file As String() = GetContent(Packget(id), pak)
                Dim OutFilePath As String = OutPath & id
#Region "AppendExtension"
                Select Case Packget(id).Extension
                    Case File.Extensions.at3
                        OutFilePath += ".at3"
                        Exit Select
                    Case File.Extensions.awb
                        OutFilePath += ".awb"
                        Exit Select
                    Case File.Extensions.bmp
                        OutFilePath += ".bmp"
                        Exit Select
                    Case File.Extensions.dat
                        OutFilePath += ".dat"
                        Exit Select
                    Case File.Extensions.font
                        OutFilePath += ".font"
                        Exit Select
                    Case File.Extensions.gim
                        OutFilePath += ".gim"
                        Exit Select
                    Case File.Extensions.gmo
                        OutFilePath += ".gmo"
                        Exit Select
                    Case File.Extensions.p3d
                        OutFilePath += ".p3d"
                        Exit Select
                    Case File.Extensions.png
                        OutFilePath += ".png"
                        Exit Select
                    Case File.Extensions.sfl
                        OutFilePath += ".sfl"
                        Exit Select
                    Case File.Extensions.vag
                        OutFilePath += ".vag"
                        Exit Select
                End Select
#End Region
                Console.Write(vbLf & "Extracting: " & System.IO.Path.GetFileName(OutFilePath))
                Dim bin As Byte() = Tools.StringToByteArray(file)
                System.IO.File.WriteAllBytes(OutFilePath, bin)
            Next
        End Sub

        Private Shared Function GetContent(file As File, pak As String()) As String()
            Dim content As String() = New String(file.EndPos - file.StartPos) {}
            For index As Integer = file.StartPos To file.EndPos
                content(index - file.StartPos) = pak(index)
            Next
            Return content
        End Function

        Private Shared Function GetFormat(pak As String(), startPos As Integer) As File.Extensions
            Dim Headers As Object() = New Object() {New Object() {"MIG.00.1PSP", File.Extensions.gim}, New Object() {"LLFS", File.Extensions.sfl}, New Object() {"RIFF", File.Extensions.at3}, New Object() {"OMG.00.1PSP", File.Extensions.gmo}, New Object() {"0x89504E47", File.Extensions.png}, New Object() {"BM", File.Extensions.bmp}, New Object() {"VAGp", File.Extensions.vag}, New Object() {"tFpS", File.Extensions.font}, New Object() {"0x41465332", File.Extensions.awb}, New Object() {"0xF0306090020000000C000000", File.Extensions.p3d}}
            For Each Header As Object In Headers
                Dim Signature As String = DirectCast(DirectCast(Header, Object())(0), String)
                Dim Extension As File.Extensions = DirectCast(DirectCast(Header, Object())(1), File.Extensions)

                Dim Hex As Boolean = Signature.StartsWith("0x")
                Dim Content As String = ""
                If Hex Then
                    Content = "0x"
                End If
                If Not Hex Then
                    For pos As Integer = 0 To Signature.Length - 1
                        Content += Tools.HexToString(pak(startPos + pos))
                    Next
                Else
                    For pos As Integer = 0 To (Signature.Length / 2) - 1
                        Content += pak(startPos + pos)
                    Next
                End If
                If Content = Signature Then
                    Return Extension
                End If
            Next
            Return File.Extensions.dat

        End Function

        Public Shared Function GetOffset(pak As String(), Position As Integer) As Integer
            Return Tools.HexToInt(pak(Position + 3) & pak(Position + 2) & pak(Position + 1) & pak(Position))
        End Function

    End Class
    Public Class File
        Public StartPos As Integer = -1
        Public EndPos As Integer = -1
        Public Extension As Extensions = Extensions.dat
        Public Enum Extensions
            gim
            sfl
            at3
            gmo
            png
            bmp
            vag
            font
            awb
            p3d
            dat
        End Enum
    End Class
    Public Class Tools
        Public Shared Function IntToHex(val As Integer) As String
            Dim hexValue As String = val.ToString("X")
            If hexValue.Length > 2 Then
                If hexValue.Length.ToString().EndsWith("1") OrElse hexValue.Length.ToString().EndsWith("3") OrElse hexValue.Length.ToString().EndsWith("5") OrElse hexValue.Length.ToString().EndsWith("7") OrElse hexValue.Length.ToString().EndsWith("9") Then
                    hexValue = "0" & hexValue
                End If
                Dim NHEX As String = ""
                For index As Integer = hexValue.Length To 0 Step -2
                    NHEX += hexValue.Substring(index - 2, 2) & " "
                Next
                NHEX = NHEX.Substring(0, NHEX.Length - 1)
                Select Case NHEX.Replace(" ", "").Length
                    Case 2
                        Return NHEX & " 00 00 00"
                    Case 4
                        Return NHEX & " 00 00"
                    Case 6
                        Return NHEX & " 00"
                    Case 8
                        Return NHEX
                End Select
                Return "null"
            Else
                If hexValue.Length = 1 Then
                    Return "0" & hexValue & " 00 00 00"
                End If
                Return hexValue & " 00 00 00"
            End If
        End Function

        Public Shared Function StringToHex(_in As String) As String
            Dim input As String = _in
            Dim values As Char() = input.ToCharArray()
            Dim r As String = ""
            For Each letter As Char In values
                Dim value As Integer = Convert.ToInt32(letter)
                Dim hexOutput As String = String.Format("{0:X}", value)
                If value > 255 Then
                    Return UnicodeStringToHex(input)
                End If
                r += value & " "
            Next
            Dim bytes As String() = r.Split(" "c)
            Dim b As Byte() = New Byte(bytes.Length - 1) {}
            Dim index As Integer = 0
            For Each val As String In bytes
                If index = bytes.Length - 1 Then
                    Exit For
                End If
                If Integer.Parse(val) > Byte.MaxValue Then
                    b(index) = Byte.Parse("0")
                Else
                    b(index) = Byte.Parse(val)
                End If
                index += 1
            Next
            r = ByteArrayToString(b)
            Return r.Replace("-", " ")
        End Function
        Public Shared Function UnicodeStringToHex(_in As String) As String
            Dim input As String = _in
            Dim values As Char() = Encoding.Unicode.GetChars(Encoding.Unicode.GetBytes(input.ToCharArray()))
            Dim r As String = ""
            For Each letter As Char In values
                Dim value As Integer = Convert.ToInt32(letter)
                Dim hexOutput As String = String.Format("{0:X}", value)
                r += value & " "
            Next
            Dim unicode As New UnicodeEncoding()
            Dim b As Byte() = unicode.GetBytes(input)
            r = ByteArrayToString(b)
            Return r.Replace("-", " ")
        End Function
        Public Shared Function StringToByteArray(hex As String) As Byte()
            Try
                Dim NumberChars As Integer = hex.Length
                Dim bytes As Byte() = New Byte(NumberChars \ 2 - 1) {}
                For i As Integer = 0 To NumberChars - 1 Step 2
                    bytes(i \ 2) = Convert.ToByte(hex.Substring(i, 2), 16)
                Next
                Return bytes
            Catch
                Console.Write("Invalid format file!")
                Return New Byte(0) {}
            End Try
        End Function
        Public Shared Function StringToByteArray(hex As String()) As Byte()
            Try
                Dim NumberChars As Integer = hex.Length
                Dim bytes As Byte() = New Byte(NumberChars - 1) {}
                For i As Integer = 0 To NumberChars - 1
                    bytes(i) = Convert.ToByte(hex(i), 16)
                Next
                Return bytes
            Catch
                Console.Write("Invalid format file!")
                Return New Byte(0) {}
            End Try
        End Function
        Public Shared Function ByteArrayToString(ba As Byte()) As String
            Dim hex As String = BitConverter.ToString(ba)
            Return hex
        End Function

        Public Shared Function HexToInt(hex As String) As Integer
            Dim num As Integer = Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber)
            Return num
        End Function

        Public Shared Function HexToString(hex As String) As String
            Dim hexValuesSplit As String() = hex.Split(" "c)
            Dim returnvar As String = ""
            For Each hexs As String In hexValuesSplit
                Dim value As Integer = Convert.ToInt32(hexs, 16)
                Dim charValue As Char = ChrW(value)
                returnvar += charValue
            Next
            Return returnvar
        End Function

        Public Shared Function UnicodeHexToUnicodeString(hex As String) As String
            Dim hexString As String = hex.Replace(" ", "")
            Dim length As Integer = hexString.Length
            Dim bytes As Byte() = New Byte(length \ 2 - 1) {}

            For i As Integer = 0 To length - 1 Step 2
                bytes(i \ 2) = Convert.ToByte(hexString.Substring(i, 2), 16)
            Next

            Return Encoding.Unicode.GetString(bytes)
        End Function

    End Class



