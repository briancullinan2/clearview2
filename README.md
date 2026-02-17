# EPIC ClearView

This was a rewrite of the original LLBLGen and Obout/Telerik and CrystalReports era ClearView application built on .Net 2.5 at the time.
This is an open source medical CRUD "template" boilerplate whatever you want to call it. It follows a bunch of fuckin rules too:

* Access Control (45 CFR § 164.312(a)) - Multiple login routes, panel level security for multi-user control, app timeouts, etc.
* Device Integrity (45 CFR § 164.312(h)) - Settings panel allows device configurations, app is restricted without a connected device.
* Audit Controls & Integrity (45 CFR § 164.312(b)-(c)) - Detailed event logging, every action, every piece of data collected.
* Electronic Records (21 CFR § 11.10)
* Electronic Signatures (21 CFR § 11.50 - 11.300)
* Arizona State Confidentiality (ARS § 12-2292)

I thought it would be cool to relive it and update it to run on Windows 10+ instead of Windows XP and show how deeply I've thought about all those rules and implemented them in a modern way.
I hope this reads like I was fired from being myself, because that's what it felt like.
All this wasted code written because my countrymen think it's more fun to bullshit around with "interviews" instead of just being normal and looking at the problem solving on full display on GitHub.
Shit I might write my own ERP, study app, and fintech app in WPF after this.

## History

I worked on this version between winter break of 2013, 4 months before getting laid off, 6 months before my contract ended, so illegally, 
but no one was around to stand up for me.
This version solved some major critical design flaws. The most major and technical, the algorithm was apparently not immune to centering issues. 
Maybe there is a bug in Maths.cs or maybe the bug it further embedded into the missing MATLAB code, which was basically just running linear regression.
The most major part I shot for an tested was using existing calibration images and passing known results and getting the same numbers.
This would mean the database and MATLAB and wiring all worked correctly compared to the previous version.
The previous version had a handful of minor but debilitating bugs that this version fixed, I'll list them now.
When a web cam was plugged in, if two applications tried to access the same camera, it would blue screen. This was a windows bug and even the
scanner add could trigger it.
When the application was opened, sometimes it would trigger this bug if the camera settings dialog was also opened. We fixed this in the 
Shell Replacement project to close the settings dialog by handle and I was planning to copy that functionality into this app.
The Database model was controlled by LLBLGen but it wasn't properly normalized so a lot of in memory data copying was happening.
I parallelized the MATLAB processing to image processing and calibrations only took 1 second instead of 10 minutes, a 6000% improvement.
I simplified some of the image loading interactions the techs used like bringing up loose patient image files in app for analysis just by clicking on the image box.
I fixed a lot of the camera interaction and allocation, I improved the timing on the camera/device charge because of threading and queuing.
I was planning on using the driver and native code to queue the image transfer in real time through device level multimedia controller, but didn't get around to it.
The installer program made some GPO changes to turn off indexing and virus protection while scanning, I was planning on running it on a specific schedule based on feedback from the app usage.
The original version had so much extra weight of UI controls that panels would flash as they load. The whole pluggable extensions that do a ton and very little at the same time was a huge burden on the whole system.

### Log

#### 2/17/2026

Finally started work on permissions but its been slow. Having fun adding templated forms tied directly to the static type on the DataLayer entity model.
Doing lots of abstracting so my templates are more concise. Trying to get general pages down to less than 200 LOC. I've abstracted the searching into a control template in a resource file.
I also abstracted the RibbonTabs into their respective resources. The nice thing it is makes it possible to override with some templating trickery I can do on page load with the resources.
The shitty thing is it makes it impossible to view the control templates in the editor. But I am betting I could override/extend ContentControl and get it to load editable template.

![Splash](./Docs/Screenshot%202026-02-17%20134653.png?raw=true)

![ClearView](./Docs/Screenshot%202026-02-17%20133251.png?raw=true)

![UserAdd](./Docs/Screenshot%202026-02-17%20133342.png?raw=true)


#### 2/14/2026

Realizing I lost some parts after dragging a bunch in from the old project. The basic timeline was, I developed the replacement app over Christmas break for a week, and then a month back in the office. This is when
I apparently copied this version back to my home computer over remote desktop and accidently pressed ctrl+c on something else before the copy finished. After this, I continued work in the office for about another 4 months.
So I've lost between 2 and 3 months worth of work on the WPF version.

I had consistent menus across the whole app. I think i continued with this contextual highlighted group panel concept for every tab. You could open multiple records at a time for patients, the names and titles were all consistent
the add user panels switched titles to Edit: User name with the name filled in, but it was basically the same page navigation. There was a back and next navigation. I think I already had a lot of the permission system worked out
but not to the degree I was planning now, it was basic tab and entry button list just for EPIC.

There was another calibration page for listing tons of calibration images vertically with their colorized and status information listed. I used this page to help calibrate the next version of the device. There was a mlformCNative.dll
that I created from within the MATLAB subscription to speed up processing I figured all the machined with Intel processors could run it, and I also built a general version too.
I added MATLAB functionality I'll probably never see again from comprehending their previous PhD code, I added the colorization functions back into the app and I added some parameter efficiency fixes.

I started to entertain the idea of using open CV to do the exact same work as MATLAB but all comprehendible from C#, I had this level of understanding of the MATLAB code and it wasn't very complicated for a seasoned mathemetician.
When I was planning the 3D organ system mapping feature I needed some extra gaming virtual worlds polymath that I have now from working on the engine, so I could probably tack this next after permissions/i18n.
All the i18n stuff in my old worked on version was loading from the database. There we're open file dialogs for loading old records off the hard drive, it was really turning in to a medical information
development tool, and that's what I'll make this one do. Maybe someday, I'll turn the app part into the life tracker or home security system I was dreaming of with a bunch of plugins and hard functionality sorted out.
Never too late for a pivot.




## TODO

* Add relevant article comments in code.
* Finished importing old panels.
* Fix DB wiring to use standard ADO.
* Finish permission auto apply feature.
* Finish i18n auto apply feature.
* Add patient data panels.
* Add user payment panels.
* Upgrade backend from IHTTP to Kestrel.
* Build new data marshaller.
* Upgrade access controls and recording.
* Reattempt WPF permission plugin using reflection on app load sequence.


## Old Website

When I started:
https://web.archive.org/web/20120204062334/http://www.epicdiagnostics.com/

When I was ended:
https://web.archive.org/web/20180125054151/http://epicdiagnostics.com/

CRU2YQSS

