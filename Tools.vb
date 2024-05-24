

Public Class Tools


 Public  Function Hash(ByVal fileName As String) As UInt64
    Dim id As UInt64 = 0
    Dim index As Integer = 0
    For Each c As Char In fileName
        If (AscW(c) >= AscW("A")) AndAlso (AscW(c) <= AscW("Z")) Then
            id = (id + (index+1) * (AscW(c) + AscW("a") - AscW("A"))) Mod &H8000000B * &HFFFFFFEF
        Else
            id = (id + (index+1) * AscW(c)) Mod &H8000000B * &HFFFFFFEF
        End If
    Next
    Return (id Xor &H12345678)
 End Function

'++i
 <Runtime.CompilerServices.Extension>
Public Function PreINC(ByRef x As Integer) As Integer
    Return System.Threading.Interlocked.Increment(x)
End Function
'i++
<Runtime.CompilerServices.Extension>
Public Function PostINC(ByRef x As Integer) As Integer
    Dim tmp = x
    System.Threading.Interlocked.Increment(x)
    Return tmp
End Function
'--i
<Runtime.CompilerServices.Extension>
Public Function PreDEC(ByRef x As Integer) As Integer
    Return System.Threading.Interlocked.Decrement(x)
End Function
i--
<Runtime.CompilerServices.Extension>
Public Function PostDEC(ByRef x As Integer) As Integer
    Dim tmp = x
    System.Threading.Interlocked.Decrement(x)
    Return tmp
End Function



End Class
