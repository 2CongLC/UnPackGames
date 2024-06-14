Imports System.Buffers
Imports System.IO

If args.Length <> 1 Then
    Console.Error.WriteLine("Usage: <input file>")
    Return 1
End If

Console.WriteLine($"Reading: {args(0)}")
Using stream As FileStream = File.OpenRead(args(0))
    Using br As BinaryReader = New BinaryReader(stream)
        Dim version As UInteger = br.ReadUInt32()
        Dim encoding As Byte = br.ReadByte()
        stream.Seek(3, SeekOrigin.Current)
        Dim resourceCount As UShort = br.ReadUInt16()
        Dim aliasCount As UShort = br.ReadUInt16()

        Dim entries As Entry() = New Entry(resourceCount + 1 - 1) {}
        Dim aliases As Alias() = New Alias(aliasCount - 1) {}

        Console.WriteLine(New With {.version = version, .encoding = encoding, .resourceCount = resourceCount, .aliasCount = aliasCount})

        For i As Integer = 0 To resourceCount
            Dim resourceId As UShort = br.ReadUInt16()
            Dim fileOffset As UInteger = br.ReadUInt32()
            entries(i) = New Entry(resourceId, fileOffset)
            Console.WriteLine(New With {.resourceId = resourceId, .fileOffset = fileOffset})
        Next

        For i As Integer = 0 To aliasCount - 1
            Dim resourceId As UShort = br.ReadUInt16()
            Dim entryIndex As UShort = br.ReadUInt16()
            aliases(i) = New Alias(resourceId, entryIndex)
            Console.WriteLine(New With {.resourceId = resourceId, .entryIndex = entryIndex})
        Next

        Const outputDirectory As String = "resources"
        Directory.CreateDirectory(outputDirectory)

        For i As Integer = 0 To resourceCount - 1
            stream.Seek(entries(i).FileOffset, SeekOrigin.Begin)
            Dim length As Integer = CInt(entries(i + 1).FileOffset - entries(i).FileOffset)
            Dim buff As Byte() = ArrayPool(Of Byte).Shared.Rent(length)
            Dim bytesRead As Integer = stream.Read(buff, 0, length)

            Using file As FileStream = File.Create(Path.Combine(outputDirectory, $"f_{entries(i).ResourceId:X4}"))
                file.Write(buff, 0, length)
            End Using

            ArrayPool(Of Byte).Shared.Return(buff)
        Next

    End Using
End Using

Return 0

Public Structure Entry
    Public ReadOnly ResourceId As UShort
    Public ReadOnly FileOffset As UInteger

    Public Sub New(resourceId As UShort, fileOffset As UInteger)
        Me.ResourceId = resourceId
        Me.FileOffset = fileOffset
    End Sub
End Structure

Public Structure Alias
    Public ReadOnly ResourceId As UShort
    Public ReadOnly EntryIndex As UShort

    Public Sub New(resourceId As UShort, entryIndex As UShort)
        Me.ResourceId = resourceId
        Me.EntryIndex = entryIndex
    End Sub
End Structure

