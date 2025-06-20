
document.addEventListener("DOMContentLoaded", function () {

    // Initialize variables as Element
    const loader = document.getElementById("hotel-loader");
    const hotelAnimationContainer = document.getElementById("hotel-animation");
    const appName = document.getElementById("app-name");
    const appTagline = document.getElementById("app-tagline");
    const brandMark = document.querySelector(".brand-mark");
    const appElement = document.getElementById("app");
    const progressBar = document.querySelector(".loading-progress-bar");

    if (!loader || !appElement) {
        console.warn("Loader or App element missing.");
        return;
    }

    // Function for showAppContent
    function showAppContent() {
        appName.classList.add('visible');
        appTagline.classList.add('visible');
        brandMark.classList.add('visible');
    }

    // Function for hide loader
    function hideLoader() {
        loader.classList.add('fade-out');
        setTimeout(() => {
            loader.style.display = 'none';
            appElement.classList.add('visible');
            window.loaderDone = true;
        }, 800);
    }

    // Load Lottie animation
    try {
        const hotelAnimation = lottie.loadAnimation({
            container: hotelAnimationContainer,
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

            // Start the progress bar animation and Smoothly animate progress bar
            progressBar.style.animation = 'none'; // Reset if needed
            setTimeout(() => {
                // Calculate animation duration based on animation length
                const animDuration = hotelAnimation.getDuration() * 1000; // Convert to ms
                const totalDuration = animDuration + 2500; // Animation + wait time
                progressBar.style.animation = `loading-progress ${totalDuration / 1000}s forwards ease-out`;
            }, 10);
        }, 300);

        // When animation completes, show our app name and dashboard for users and then hide loader
        hotelAnimation.addEventListener('complete', () => {
            showAppContent();

            // add the waiting time = 2.5s and hide loader
            setTimeout(hideLoader, 2500);
        });

        // Fallback timeout if animation hangs or never completes
        setTimeout(() => {
            if (!window.loaderDone && loader.style.display !== 'none') {
                showAppContent();

                setTimeout(hideLoader, 1500); // add the waiting time = 2.5s
            }
        }, 6000);

    } catch (error) {
        console.error("Error loading animation: failed ", error);

        // Show our app immediately if animation fails
        showAppContent();

        // Set a fixed animation for the progress bar in case of error
        progressBar.style.animation = 'loading-progress 3s forwards ease-out';

        // Hide loader after a short delay and add the waiting time = 2s
        setTimeout(hideLoader, 2000);
    }
});
