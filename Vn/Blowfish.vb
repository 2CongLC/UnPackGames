'https://github.com/svalencius/pk2-reader/blob/master/app/pk2tools/blowfish/Blowfish.js
Imports System.IO

Public Class Blowfish
    Private PArray As Integer()
    Private SBoxes As Integer()()

    Public Function Blowfish_decipher(xl As Integer, xr As Integer) As Dictionary(Of String, Integer)
        Dim Xl As Integer = xl
        Dim Xr As Integer = xr
        Xl = Xl Xor Me.PArray(17)
        Xl = Helper.toUInt32(Xl)
        Xr = Me.ROUND(Xr, Xl, 16)
        Xl = Me.ROUND(Xl, Xr, 15)
        Xr = Me.ROUND(Xr, Xl, 14)
        Xl = Me.ROUND(Xl, Xr, 13)
        Xr = Me.ROUND(Xr, Xl, 12)
        Xl = Me.ROUND(Xl, Xr, 11)
        Xr = Me.ROUND(Xr, Xl, 10)
        Xl = Me.ROUND(Xl, Xr, 9)
        Xr = Me.ROUND(Xr, Xl, 8)
        Xl = Me.ROUND(Xl, Xr, 7)
        Xr = Me.ROUND(Xr, Xl, 6)
        Xl = Me.ROUND(Xl, Xr, 5)
        Xr = Me.ROUND(Xr, Xl, 4)
        Xl = Me.ROUND(Xl, Xr, 3)
        Xr = Me.ROUND(Xr, Xl, 2)
        Xl = Me.ROUND(Xl, Xr, 1)
        Xr = Xr Xor Me.PArray(0)
        Xr = Helper.toUInt32(Xr)
        xl = Xr
        xr = Xl

        Dim rtn As New Dictionary(Of String, Integer) From {
            {"xr", xr},
            {"xl", xl}
        }
        Return rtn
    End Function

    Public Sub Initialize(key_ptr As Byte())
        Dim i, j, length As Integer
        Dim data, datal, datar As Integer
        length = key_ptr.Length

        For i = 0 To 17
            Me.PArray(i) = Me.bf_P(i)
        Next

        For i = 0 To 3
            For j = 0 To 255
                Me.SBoxes(i)(j) = Me.bf_S(i)(j)
            Next
        Next

        Dim tmp(3) As Byte
        j = 0
        For i = 0 To 16 + 1
            tmp(3) = key_ptr(j)
            tmp(2) = key_ptr((j + 1) Mod length)
            tmp(1) = key_ptr((j + 2) Mod length)
            tmp(0) = key_ptr((j + 3) Mod length)
            data = BitConverter.ToInt32(tmp, 0)
            Me.PArray(i) = Me.PArray(i) Xor data
            Me.PArray(i) = Helper.toUInt32(Me.PArray(i))
            j = (j + 4) Mod length
        Next

        datal = 0
        datar = 0

        For i = 0 To 16 + 1 Step 2
            Dim t = Me.Blowfish_encipher(datal, datar)
            datal = t("xl")
            datar = t("xr")
            Me.PArray(i) = datal
            Me.PArray(i + 1) = datar
        Next

        For i = 0 To 3
            For j = 0 To 255 Step 2
                Dim t = Me.Blowfish_encipher(datal, datar)
                datal = t("xl")
                datar = t("xr")
                Me.SBoxes(i)(j) = t("xl")
                Me.SBoxes(i)(j + 1) = t("xr")
            Next
        Next
    End Sub

    Public Function GetOutputLength(length As Integer) As Integer
        Return If((length Mod 8) = 0, length, length + (8 - (length Mod 8)))
    End Function

    Public Function Encode(stream As Byte(), offset As Integer, length As Integer) As Dictionary(Of String, Object)
        If length = 0 Then
            Console.WriteLine("ERROR Length = 0!")
        End If

        Dim workspaceLength As Integer = Me.GetOutputLength(length)
        Dim workspace(workspaceLength - 1) As Byte
        Array.Copy(stream, offset, workspace, 0, length)

        For x = 0 To workspaceLength - 1 Step 8
            Dim l As Integer = BitConverter.ToInt32(workspace, x)
            Dim r As Integer = BitConverter.ToInt32(workspace, x + 4)
            Dim t = Me.Blowfish_encipher(l, r)
            l = t("xl")
            r = t("xr")
            Array.Copy(BitConverter.GetBytes(l), 0, workspace, x, 4)
            Array.Copy(BitConverter.GetBytes(r), 0, workspace, x + 4, 4)
        Next

        Dim obj As New Dictionary(Of String, Object) From {
            {"size", workspaceLength},
            {"buff", workspace}
        }
        Return obj
    End Function

    Public Function Decode(stream As Byte(), offset As Integer, length As Integer) As Byte()
        If length Mod 8 <> 0 OrElse length = 0 Then
            Console.WriteLine("ERROR invalid Length")
        End If

        Dim workspace(length - 1) As Byte
        Array.Copy(stream, offset, workspace, 0, length)

        For x = 0 To length - 1 Step 8
            Dim l As Integer = BitConverter.ToInt32(workspace, x)
            Dim r As Integer = BitConverter.ToInt32(workspace, x + 4)
            Dim t = Me.Blowfish_decipher(l, r)
            l = t("xl")
            r = t("xr")
            Array.Copy(BitConverter.GetBytes(l), 0, workspace, x, 4)
            Array.Copy(BitConverter.GetBytes(r), 0, workspace, x + 4, 4)
        Next

        Return workspace
    End Function
End Class

Public Class Helper
    Public Shared Function toUInt32(value As Integer) As Integer
        Return value And &HFFFFFFFFUI
    End Function
End Class


