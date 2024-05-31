Module cryptor

Sub EDOneTimePad_Encipher(ByRef pPlaintext As Char(), ByVal nPlainLen As Integer)
    If pPlaintext IsNot Nothing AndAlso nPlainLen > 0 Then
        Dim pContent As Byte() = New Byte(nPlainLen - 1) {}
        Array.Copy(DirectCast(pPlaintext, Byte()), pContent, nPlainLen)
        Const NUM_FIX_CIPHER_CHARA As Integer = 3
        Dim cCipher As Byte() = New Byte(NUM_FIX_CIPHER_CHARA + NUM_FIX_CIPHER_CHARA - 1) {CByte("W"c), CByte("o"c), CByte("o"c), CByte("y"c), CByte("u"c), CByte("e"c)}
        If nPlainLen > NUM_FIX_CIPHER_CHARA Then
            cCipher(0) = CByte(pContent(nPlainLen - 1) Mod 4)
            If cCipher(0) <> 0 Then
                cCipher(0) = cCipher(cCipher(0) + 2)
            Else
                cCipher(0) = pContent(nPlainLen - 1)
            End If

            If nPlainLen > NUM_FIX_CIPHER_CHARA + 1 Then
                cCipher(1) = CByte("l"c)
                If nPlainLen > NUM_FIX_CIPHER_CHARA + 2 Then
                    cCipher(2) = pContent(nPlainLen - 2)
                    If nPlainLen > NUM_FIX_CIPHER_CHARA + 3 Then
                        cCipher(1) = pContent(nPlainLen - 3)
                    End If
                End If
            End If
        End If

        For nPos As Integer = 0 To nPlainLen - 1
            pContent(nPos) = CByte(((pContent(nPos) - &H20 + (&HFF - cCipher(cCipher(0) Mod 3))) Mod &HDF) + &H20)
            cCipher(0) = cCipher(1)
            cCipher(1) = cCipher(2)
            cCipher(2) = pContent(nPos)
        Next

        Array.Copy(pContent, DirectCast(pPlaintext, Byte()), nPlainLen)
    End If
End Sub

Sub EDOneTimePad_Decipher(ByRef pCiphertext As Char(), ByVal nCiphertextLen As Integer)
    If pCiphertext IsNot Nothing AndAlso nCiphertextLen > 0 Then
        Dim pContent As Byte() = New Byte(nCiphertextLen - 1) {}
        Array.Copy(DirectCast(pCiphertext, Byte()), pContent, nCiphertextLen)
        Const NUM_FIX_CIPHER_CHARA As Integer = 3
        Dim cCipher As Byte() = New Byte(NUM_FIX_CIPHER_CHARA + NUM_FIX_CIPHER_CHARA - 1) {CByte("W"c), CByte("o"c), CByte("o"c), CByte("y"c), CByte("u"c), CByte("e"c)}

        For nPos As Integer = nCiphertextLen - 1 To NUM_FIX_CIPHER_CHARA Step -1
            pContent(nPos) = CByte(((pContent(nPos) - &H20 + (pContent(nPos - 3 + (pContent(nPos - 3) Mod 3)) - &H20)) Mod &HDF) + &H20)
        Next

        If nCiphertextLen > NUM_FIX_CIPHER_CHARA Then
            cCipher(0) = CByte(pContent(nCiphertextLen - 1) Mod 4)
            If cCipher(0) <> 0 Then
                cCipher(0) = cCipher(cCipher(0) + 2)
            Else
                cCipher(0) = pContent(nCiphertextLen - 1)
            End If

            If nCiphertextLen > NUM_FIX_CIPHER_CHARA + 1 Then
                cCipher(1) = CByte("l"c)
                If nCiphertextLen > NUM_FIX_CIPHER_CHARA + 2 Then
                    cCipher(2) = pContent(nCiphertextLen - 2)
                    If nCiphertextLen > NUM_FIX_CIPHER_CHARA + 3 Then
                        cCipher(1) = pContent(nCiphertextLen - 3)
                    End If
                End If
            End If
        End If

        If nPos >= 0 Then
            Array.Copy(pContent, 0, cCipher, NUM_FIX_CIPHER_CHARA, nPos + 1)
        End If

        For nPos = nPos To 0 Step -1
            pContent(nPos) = CByte(((pContent(nPos) - &H20 + (cCipher(nPos + (cCipher(nPos) Mod 3)) - &H20)) Mod &HDF) + &H20)
        Next

        Array.Copy(pContent, DirectCast(pCiphertext, Byte()), nCiphertextLen)
    End If
End Sub



End Module  
