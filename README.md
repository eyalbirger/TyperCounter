# TyperCounter
**⌨️ TyperCounter**  
**TyperCounter** is a lightweight C# app designed for custom keyboards enthusiasts. It records your all of your keyboard outputs and transforms that data into a visual heatmap using the [keyboard layout editor](https://www.keyboard-layout-editor.com/#/) format

**Why TyperCounter**  
Recently, I have started to build a lot of small split keyboards. Those small keyboards require using multiple layers (in a regular keyboard there are layers like: fn, shift, ctrl) for regular keys on a regular keyboard (like numbers, symbols, F keys). I wanted to know what keys I use the most (apart from the regular letters) so I can I them more accessible then others.

**Is this secure?**  
Yes. The gitignore doesn't allow you to submit the text file that records your text file.



# Features

- ⌨️ Automatic key-logging.
- 💻 A clean, simple and lightweight Terminal User Interface (TUI).
- 🤖 Automated key-mapping.
- 👀 Quick clean and detailed review.
- 🔥 Automated heatmap creator for every keyboard layout.
- 🔐 A secure app so you won't accidentally upload your logs to github :).
- 🌍 Global compatibility- works with ANY keyboard layout.
- 💾 Easy install and usage.
  
# Install and Usage
**Pre installation requisites**  
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)  

**installation**  
[Video guide](https://youtu.be/dQw4w9WgXcQ?si=VwlQpzGtQ58pnWTe)  

**quick guide:**  
&emsp;**1: Clone The Repo:**  
```
git clone https://github.com/YOUR_USERNAME/TyperCounter.git
cd TyperCounter/Code/TyperCounter
```  
&emsp;**2: Copy & Paste your keyboard layout editors "Raw Data" into a new file called:**  
```layout.txt```

&emsp;**3: Run the app**  
```
dotnet run
```  

&emsp;**4: Generate Your HeatMap**  
&emsp;Follow the app's instructions. The app will create a layoutOutpus.txt file in your ```layout/``` folder.   

&emsp;**5: Visualize**  
&emsp;Copy the contents of layoutOutput.txt and paste it into the Raw Data tab at keyboard-layout-editor.com.