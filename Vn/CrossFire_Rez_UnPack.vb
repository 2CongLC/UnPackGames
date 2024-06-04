Imports System
Imports System.Collections
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime
Imports System.Text
Imports System.Text.RegularExpressions


Module Program

    Public br As BinaryReader
    Public input As String
    Sub Main(args As String())
        If args.Count = 0 Then
            Console.WriteLine("Tool UnPack - 2CongLC.vn :: 2024")
        Else
            input = args(0)
        End If
        Dim p As String = Nothing
        If IO.File.Exists(input) Then

            br = New BinaryReader(File.OpenRead(input))
            Dim CR1 As Char = br.ReadChar ' Offset = 0, Length = 2
            Dim LF1 As Char = br.ReadChar ' Offset = 2, Length = 2
            Dim FileType As String = New String(br.ReadChars(60)) ' Offset = 4, Length = 60
            Dim CR2 As Char = br.ReadChar ' Offset = 64, Length = 2
            Dim LF2 As Char = br.ReadChar ' Offset = 66, Length = 2
            Dim UserTitle As String = New String(br.ReadChars(60)) ' Offset = 68, Length = 60
            Dim CR3 As Char = br.ReadChar ' Offset = 128, Length = 2
            Dim LF3 As Char = br.ReadChar ' Offset = 130, Length = 2
            Dim EOF1 As Char = br.ReadChar ' Offset = 132, Length = 2
            Dim FileFormatVersion As UInt32 = br.ReadUInt32 ' Offet = 134, Length = 4
            Dim RootDirPos As UInt32 = br.ReadUInt32 ' Offset = 138, Length = 4
            Dim RootDirSize As UInt32 = br.ReadUInt32 ' Offset = 142, Length = 4
            Dim RootDirTime As UInt32 = br.ReadUInt32 ' Offset = 146, Length = 4
            Dim NextWritePos As UInt32 = br.ReadUInt32 ' Offset = 150, Length = 4
            Dim Time As UInt32 = br.ReadUInt32 ' Offset = 154, Length = 4
            Dim LargestKeyAry As UInt32 = br.ReadUInt32 ' Offset = 158, Length = 4
            Dim LargestDirNameSize As UInt32 = br.ReadUInt32 ' Offset = 162, Length = 4
            Dim LargestRezNameSize As UInt32 = br.ReadUInt32 ' Offset = 166, Length = 4
            Dim LargestCommentSize As UInt32 = br.ReadUInt32 ' Offset = 170, Length = 4
            Dim IsSorted As Byte = br.ReadByte ' Offset = 174, Length = 1

            br.BaseStream.Seek(RootDirPos, SeekOrigin.Begin)
            Dim buffer as Byte() = New Byte(RootDirSize){}
            br.Read(buffer, 0, RootDirSize)
            Decrypt(buffer, RootDirSize, RootDirPos)
            Dim ms as MemoryStream = New MemoryStream(buffer)
            ms.Position = 0
            br = New BinaryReader(ms)
            Dim types as Int32 = br.ReadInt32
            br.BaseStream.Position += 4
            Console.WriteLine("Data Type : {0}",types)
            Dim subtables as New List(Of TableData)()
            Dim subfiles as New List(Of FileData)()
            If Types = 1 Then
                subtables.Add(New TableData)
                br.BaseStream.Position += 16
            Else
                subfilea.Add(New FileData)
                br.BaseStream.Position += 28
            End If 

            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
            'Directory.CreateDirectory(p)
            Dim n as String = Nothing
            Dim extension as String = Nothing

            For Each td as TableData in subtables
                n = td.name
            Next

            For Each fd as FileData in subfiles
                Dim ext As String = New String(BitConverter.GetBytes(fd.ext).Reverse().Select(Function(c) CChar(c)).ToArray())
                
            Next

           
        End If
    End Sub

    Class TableData
        Dim pos as UInt32 ' Length = 4
        Dim size as UInt32 ' Length = 4
        Dim time as UInt32 ' Length = 4
        Dim namelen as String ' Length = 4
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReadUInt32
            namelen = New String(br.ReadChars(4))
        End Sub
    End Class
    
    Class FileData
        Dim pos as UInt32 ' Length = 4
        Dim size as UInt32 ' Length = 4
        Dim time as UInt32 ' Length = 4
        Dim id as UInt32 ' Length = 4
        Dim ext as UInt32 ' Length = 4
        Dim numkeys as UInt32 ' Length = 4
        Dim namelen as String ' Length = 4
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReadUInt32
            id = br.ReadUInt32
            ex = br.ReadUInt32
            numkeys = br.ReadUInt32
            namelen = New String(br.ReadChars(4))
        End Sub    
    End Class

    Public Sub Decrypt(ByVal buf As Char(), ByVal size As Integer, ByVal keyPos As Integer)
        For i As Integer = 0 To size - 1
            buf(i) = CChar(Convert.ToInt32(49) + (KEY(keyPos Mod KEY.Length) Xor (Not buf(i))))
            keyPos += 1
        Next
    End Sub

    

End Module      
