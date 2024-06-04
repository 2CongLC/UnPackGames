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
            Dim CR1 As Char
            Dim LF1 As Char
            Dim FileType As String
            Dim CR2 As Char
            Dim LF2 As Char
            Dim UserTitle As String
            Dim CR3 As Char
            Dim LF3 As Char
            Dim EOF1 As Char
            Dim FileFormatVersion As UInteger
            Dim RootDirPos As UInteger
            Dim RootDirSize As UInteger
            Dim RootDirTime As UInteger
            Dim NextWritePos As UInteger
            Dim Time As UInteger
            Dim LargestKeyAry As UInteger
            Dim LargestDirNameSize As UInteger
            Dim LargestRezNameSize As UInteger
            Dim LargestCommentSize As UInteger
            Dim IsSorted As Byte





        End If
    End Sub
    Class FileData

    End Class

    

End Module      
