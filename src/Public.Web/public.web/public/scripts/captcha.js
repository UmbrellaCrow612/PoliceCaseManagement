console.log("Script.js loaded");

let botChanceScore = 0;
const toggleButtonId = "captcha";
const toggleButtonCheckedAttribute = "captcha-checked";
const previousMousePositionArray = [];
const MAX_MOUSE_POSITION_ENTRIES = 1000;

function analyzeMouseMovements() {
  console.log(previousMousePositionArray)
  return 0.2;
}

function analyzeClick() {
  // this will how long click time took for toggle button to be clicked super fast will mean high score slower mean less score
  /*
       let timer;
        let startTime;

        window.onload = () => {
            // Start the timer when the page loads
            startTime = Date.now();
            timer = setInterval(() => {
                // Optionally, you can do something on every tick of the timer here.
            }, 1000); // 1 second interval (1000 milliseconds)
        };

        document.getElementById("startButton").onclick = () => {
            // Stop the timer when the button is clicked
            clearInterval(timer);
            
            // Calculate how long it took to click the button
            const timeTaken = (Date.now() - startTime) / 1000; // time in seconds
            console.log(`It took you ${timeTaken.toFixed(2)} seconds to click the button.`);
        };
  */
  return 0.1;
}

function analyzeBrowser() {
  return 0.1;
}

function analyzeNetwork() {
  return 0.1;
}

function analyzeJavaScriptChallenges() {
  return 0.1;
}

document.addEventListener("mousemove", (event) => {
  const mouseX = event.clientX;
  const mouseY = event.clientY;

  previousMousePositionArray.push({
    x: mouseX,
    y: mouseY,
    time: performance.now(),
  });

  if (previousMousePositionArray.length > MAX_MOUSE_POSITION_ENTRIES) {
    previousMousePositionArray.splice(
      0,
      previousMousePositionArray.length - MAX_MOUSE_POSITION_ENTRIES
    );
    botChanceScore = Math.max(0, botChanceScore - 0.1);
  }
});

const captchaToggleButton = document.getElementById(toggleButtonId);
if (captchaToggleButton) {
  captchaToggleButton.addEventListener("click", () => {
    let checked = captchaToggleButton.getAttribute(
      toggleButtonCheckedAttribute
    );

    try {
      checked = JSON.parse(checked);
    } catch {
      throw new Error(
        "Captcha checkbox value is not valid JSON. Unable to parse."
      );
    }

    if (typeof checked !== "boolean") {
      throw new Error("Captcha checkbox value is not a boolean.");
    }

    if (!checked) {
      if (botChanceScore >= 0.8) {
        console.log(
          "High chance of a bot clicking this - toggle a captcha now."
        );

        // here just trigger a captcha
        // on the document model attacked a isBot that clients use to trigger captcha
      } else {
        console.log("Human interaction detected.");
      }
    }
  });
} else {
  throw new Error("Captcha toggle button not found in the DOM.");
}

setInterval(() => {
  console.log("setInterval ran");
  const mouseMovementScore = analyzeMouseMovements();
  const analyzeClickScore = analyzeClick();
  const analyzeBrowserScore = analyzeBrowser();
  const analyzeNetworkScore = analyzeNetwork();
  const analyzeJavaScriptChallengesScore = analyzeJavaScriptChallenges();

  const cm =
    mouseMovementScore +
    analyzeClickScore +
    analyzeBrowserScore +
    analyzeNetworkScore +
    analyzeJavaScriptChallengesScore;
  botChanceScore += Math.min(1, cm);
}, 5000);
