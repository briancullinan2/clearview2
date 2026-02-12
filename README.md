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
The previous version had a handful of minor but dibilitating bugs that this version fixed, I'll list them now.
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

