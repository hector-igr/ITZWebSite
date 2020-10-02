// eslint-disable-next-line @typescript-eslint/no-namespace
namespace HGR_DOM {

    class ForgeAppDOM {

        public static Ini(): void {
            ForgeAppDOM.SubscribeElement();

            console.log("ForgeAppDOM.INI")
        }

        static SubscribeElement(): void {
            const root = document.getElementById("myDiv");

            //SCROLL DEBUGING
            //root.addEventListener("scroll", function (e) {
            //    ScrollingRountines.ChangeElmentContent("logo", root.scrollTop.toString())
            //});

            //TOGGLE CLASSES ON MODULE APEARENCE
            {
                const parents = [...root.getElementsByClassName("project-cover")].map(x => x as HTMLElement);
                for (let p = 0; p < parents.length; p++) {
                    const parent = parents[p];
                    const header = parent.getElementsByClassName("projectHeader")[0] as HTMLElement;
                    ScrollingRountines.ChangeChildrenAnimationByScrolling(root, parent, header, "hideContentHeader", "showContentHeader");
                    header.style.fontSize = "110px";

                    const projectsInfo = parent.getElementsByClassName("projectInfo")[0] as HTMLElement;
                    ScrollingRountines.ChangeChildrenAnimationByScrolling(root, parent, projectsInfo, "hideContentInfo", "showContentInfo");
                }
            }

            //CHANGE THE HEADER AND THE PANE BACKGROUND ON FREESPACE MOUSEOVER
            {
                const modules = root.getElementsByClassName("horizontalModule");
                //console.log(modules);
                for (let m = 0; m < modules.length; m++) {
                    const module = modules[m] as HTMLElement;
                    const rail = module.parentElement;
                    const header = rail.getElementsByClassName("projectHeader")[0] as HTMLElement;
                    const info = rail.getElementsByClassName("projectInfo")[0] as HTMLElement;
                    const pane = module.getElementsByClassName("paneInteraction")[0] as HTMLElement;

                    const freeSpace = module.getElementsByClassName("freeZone")[0] as HTMLElement;
                    if (freeSpace) {
                        freeSpace.addEventListener("mouseover", function (e) {
                            //console.log("mouseover a freezone");
                            header.style.fontSize = "50px";

                            pane.style.opacity = "0.2";

                        });
                        freeSpace.addEventListener("mouseout", function (e) {
                            //console.log("mouseout a freezone");
                            header.style.fontSize = "90px";

                            pane.style.opacity = "1.0";
                        });
                    }
                }
            }


            //IMAGE RAIL
            {
                const horizotalRails = root.getElementsByClassName("horizontalRail");
                //console.log("horizontal Rails : " + horizotalRails.length);
                for (let r = 0; r < horizotalRails.length; r++) {
                    const rail = horizotalRails[r] as HTMLElement;
                    const modules = rail.getElementsByClassName("horizontalModule");
                    //console.log("   modules : " + modules.length, modules);

                    for (let m = 0; m < modules.length; m++) {
                        const indexIni = m - 1 >= 0 ? m - 1 : modules.length - 1;
                        const previousModule = modules[indexIni] as HTMLElement;
                        const currentModule = modules[m] as HTMLElement;
                        const indexEnd = m + 1 <= modules.length - 1 ? m + 1 : 0;
                        const nextModule = modules[indexEnd] as HTMLElement;

                        const rollBack = currentModule.getElementsByClassName("rollBack")[0];
                        const rollFront = currentModule.getElementsByClassName("rollFront")[0];

                        if (rollBack && previousModule) {
                            rollBack.addEventListener("click", function (e) {
                                console.log("onclick back", previousModule);
                                previousModule.scrollIntoView({ block: "start", behavior: "smooth" });
                                ScrollingRountines.ScrollToElement(rail, previousModule);
                            });
                        }

                        if (rollFront && nextModule) {
                            rollFront.addEventListener("click", function (e) {
                                console.log("onclick front", nextModule);
                                nextModule.scrollIntoView({ block: "start", behavior: "smooth" });
                                ScrollingRountines.ScrollToElement(rail, previousModule);
                            });
                        }
                    }
                }
            }

            const covers = [...root.getElementsByClassName("project-cover")].map(x => x as HTMLElement);
            for (let c = 0; c < covers.length; c++) {
                const cover = covers[c];
                const info = cover.getElementsByClassName("projectInfo")[0] as HTMLElement;
                if (info === null) continue;

                const modules = [...cover.getElementsByClassName("horizontalModule")].map(x => x as HTMLElement);
                //console.log(modules);
                for (let i = 0; i < modules.length; i++) {

                    const module = modules[i];
                    //const imgPath = module.style.backgroundImage.replace('url(', '').replace(')', '').replace('"', '').replace('"', '');
                    //console.log(imgPath);
                    const observer = new IntersectionObserver(function (entries, ) {
                        const module = entries[0].target as HTMLElement;
                        const backColor = module.getAttribute("backColor");

                        if (entries[0].isIntersecting === true && entries[0].intersectionRatio >= 0.5) {
                            const backs = document.getElementsByClassName("imageRibbon");
                            for (let i = 0; i < backs.length; i++) {
                                const background = backs[i] as HTMLElement;
                                if (backColor !== null && backColor.length > 0) {
                                    background.style.backgroundColor = "rgb(" + backColor + ")";
                                }
                            }

                            const paneInter = document.getElementsByClassName("paneInteraction");
                            for (let i = 0; i < paneInter.length; i++) {
                                const background = paneInter[i] as HTMLElement;
                                if (backColor !== null && backColor.length > 0) {
                                    background.style.backgroundImage = `linear-gradient(to bottom, rgba(${backColor},1), transparent, rgba(${backColor},1))`;
                                }
                            }

                            const commentNode = module.getElementsByClassName("moduleComments")[0];
                            if (commentNode !== null) {

                                if (module.classList.contains("firstModule")) {
                                    ScrollingRountines.ToggleClasses(info, "showContentInfo", "bounceContentInfo");
                                }
                                else if (info.classList.contains("bounceContentInfo")) {
                                    ScrollingRountines.ToggleClasses(info, "bounceContentInfo", "bounceContentInfo");
                                }
                                else {
                                    ScrollingRountines.ToggleClasses(info, "showContentInfo", "bounceContentInfo");
                                }

                                const content = commentNode.innerHTML;
                                info.innerHTML = content;
                                //console.log("BEFORE BOUNCE");
                            }
                        }

                        //pause and play videos
                        const videos = module.getElementsByTagName("video");
                        if (videos.length > 0) {
                            const vid = videos[0];
                            //const src = vid.getElementsByTagName("source")[0].src;
                            //console.log(src + " : " + entries[0].intersectionRatio.toString());

                            if (entries[0].intersectionRatio >= 0.5) {
                                vid.play();
                                //console.log("play video : " + src);
                            }
                            else {
                                vid.pause(); const root = document.getElementById("myDiv");
                                //console.log("pause video : " + src);
                            }
                        }
                    }, { threshold: [0, 0.25, 0.5, 0.75, 1] });
                    observer.observe(module);
                }

                //let videos = document.querySelectorAll('video');
                //let isPaused = false; /* Flag for auto-paused video */
                //let observer = new IntersectionObserver((entries, ooo) => {
                //    entries.forEach(entry => {
                //        let video = entry.target as HTMLElement;
                //        if (entry.intersectionRatio != 1 && !video.paused) {
                //            video.pause(); isPaused = true;
                //        }
                //        else if (isPaused) { video.play(); isPaused = false }
                //    });
                //}, { threshold: 1 });
                //observer.observe(videos);


            }
        }

        static SubscriveVideoHover(): void {

            const videos = document.getElementsByClassName("showControlsOnHover");
            for (var i = 0; i < videos.length; i++) {
                const video = videos[i] as HTMLElement;
                video.removeAttribute("controls");
                video.addEventListener("mouseenter", function (e) {
                    video.setAttribute("controls", "controls");
                });
                video.addEventListener("mouseleave", function (e) {
                    video.removeAttribute("controls");
                });
            }
        }

        static ReloadVideo(videoElId: string): void {
            try {
                const el = document.getElementById(videoElId) as HTMLVideoElement;
                if (el !== null) {
                    el.load();
                }
            } catch (e) {

            }
            
        }
    }

    class Util_DOM_Debug {

        public static PrintTreeInfo(element: HTMLElement) {
            let el = element;
            let elements: HTMLElement[] = [];
            while (el != null) {
                elements.push(el);
                if (el.parentElement !== undefined) {
                    el = el.parentElement;
                }
                else {
                    break;
                }
            }
            let count: number = 0;
            for (var i = elements.length - 1; i >= 0; i--) {
                //console.log(count, elements[i], elements[i].offsetTop, elements[i].clientHeight);
                count++;
            }
        }
    }

    class ScrollingRountines {

        public static ChangeElmentContent(id: string, content: string) {
            const el = document.getElementById(id);
            if (el) {
                el.innerHTML = content;
            }
        }

        public static ContentVisibilityOnScroll(el: HTMLElement): void {
            const position = el.getBoundingClientRect();
            const top = position.top;
        }


        /**
         * Subscribe children nodes that satisfy the filterClass inside a html parent that its scrollabe
        toggleing classes
        */
        public static ChangeChildrenAnimationByScrolling(root: HTMLElement, parent: HTMLElement, child: HTMLElement,
            initialClass: string, finalClass: string): void {

            const rootMid = (root.clientHeight / 2) + root.offsetTop;
            const parentOffset = ScrollingRountines.GetOffsetTopFromParent(root, parent);
            const parentMiddle = parentOffset + (parent.clientHeight / 2);
            const boundary = parentMiddle - rootMid -5; /*5 px for pushing a little before get near the boundary*/

            root.addEventListener("scroll", function (e) {
                ScrollingRountines.ToggleClassessByScrolling(child, initialClass, finalClass, root, boundary);
            });
        }

        public static ToggleClassessByScrolling(el: HTMLElement, originalClass: string, toggleClass: string,
            parent: HTMLElement, offsetHeight: number): void {
            if (el.classList.contains(originalClass) || el.classList.contains(toggleClass)) {
                if (parent.scrollTop > offsetHeight) {
                    el.classList.remove(originalClass);

                    el.classList.add(toggleClass);
                }
                else {
                    el.classList.remove(toggleClass);

                    el.classList.add(originalClass);
                }
            }
        }

        public static ToggleClasses(el: HTMLElement, originalClass: string, toggleClass: string) {
            if (el.classList.contains(originalClass)) {
                el.classList.remove(originalClass);
                void el.offsetWidth;
                el.classList.add(toggleClass);
            }
            else if (el.classList.contains(toggleClass)) {
                el.classList.remove(toggleClass);
                void el.offsetWidth;
                el.classList.add(originalClass);
            }
        }

        public static GetOffsetTopFromParent(parent: HTMLElement, child: HTMLElement): number {
            let height = 0.00;
            let c = child;
            while (true) {
                if (c.isSameNode(parent)) {
                    break;
                }
                else {
                    height += c.offsetTop;
                    //console.log(c.offsetParent);
                    if (c.offsetParent) {
                        c = c.offsetParent as HTMLElement;
                    }
                    else {
                        break;
                    }
                }
            }
            return height;
        }

        public static ScrollToElement(parent: HTMLElement, el: HTMLElement) {
            parent.click = () => {
                console.log("ScrollToElement ...");
                el.scrollIntoView();
            };
        }
    }

    //Is a must, classes are needed in the windows object for consuming
    export function Load(): void {
        window["ForgeAppDOM"] = ForgeAppDOM;
        window["Util_DOM_Debug"] = Util_DOM_Debug;
        window["ScrollingRountines"] = ScrollingRountines;
    }
}

HGR_DOM.Load();