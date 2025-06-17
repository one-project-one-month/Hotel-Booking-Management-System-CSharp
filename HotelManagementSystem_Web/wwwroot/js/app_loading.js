
// Initialize variables
let hotelAnimation = null;
const loader = document.getElementById("hotel-loader");
const appName = document.getElementById("app-name");
const appTagline = document.getElementById("app-tagline");
const brandMark = document.querySelector(".brand-mark");
const appElement = document.getElementById("app");
const progressBar = document.querySelector(".loading-progress-bar");

// Function to hide loader
function hideLoader() {
    loader.classList.add('fade-out');
    setTimeout(() => {
        if (loader) {
            loader.style.display = 'none';
            appElement.classList.add('visible');
        }
    }, 800);
}

// Try to load the animation
try {
    hotelAnimation = lottie.loadAnimation({
        container: document.getElementById('hotel-animation'),
        renderer: 'svg',
        loop: false,
        autoplay: false,
        path: 'Hotel_Animation.json',
        rendererSettings: {
            progressiveLoad: false,
            preserveAspectRatio: 'xMidYMid meet',
            clearCanvas: true
        }
    });

    // Start animation after a short delay
    setTimeout(() => {
        hotelAnimation.play();

        // Start the progress bar animation programmatically
        progressBar.style.animation = 'none'; // Reset animation
        setTimeout(() => {
            // Calculate animation duration based on animation length
            const animDuration = hotelAnimation.getDuration() * 1000; // Convert to ms
            const totalDuration = animDuration + 2500; // Animation + wait time
            progressBar.style.animation = `loading-progress ${totalDuration / 1000}s forwards ease-out`;
        }, 10);
    }, 300);

    // When animation completes, show app name then hide loader
    hotelAnimation.addEventListener('complete', () => {
        // Show name and tagline
        appName.classList.add('visible');
        appTagline.classList.add('visible');
        brandMark.classList.add('visible');

        // Wait 2.5s and hide loader
        setTimeout(hideLoader, 2500);
    });

    // Fallback if animation takes too long
    setTimeout(() => {
        if (loader.style.display !== 'none') {
            appName.classList.add('visible');
            appTagline.classList.add('visible');
            brandMark.classList.add('visible');
            setTimeout(hideLoader, 1500); // Wait 1.5s
        }
    }, 6000);
} catch (error) {
    console.error("Error loading animation:", error);
    // Show app name immediately if animation fails
    appName.classList.add('visible');
    appTagline.classList.add('visible');
    brandMark.classList.add('visible');

    // Set a fixed animation for the progress bar in case of error
    progressBar.style.animation = 'loading-progress 3s forwards ease-out';

    // Hide loader after a short delay
    setTimeout(hideLoader, 2000);
}
