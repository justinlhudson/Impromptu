FROM ubuntu:12.04

RUN adduser developer && adduser developer sudo
RUN echo "developer:password" | chpasswd

# Workaround for upstart errors
RUN apt-mark hold initscripts udev plymouth mountall ifupdown procps
RUN dpkg-divert --local --rename --add /sbin/initctl;ln -sf /bin/true /sbin/initctl || true

# Allow it to work regardless of locale/language set on the users machine
RUN locale-gen en_US en_US.UTF-8 && dpkg-reconfigure locales

ADD sources.list /etc/apt/sources.list
RUN apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 3E5C1192

RUN mkdir -p /var/run/sshd; mkdir -p /var/log/supervisor

RUN apt-get update -y; apt-get install vim sudo expect openssh-server supervisor -y
RUN apt-get update -y; apt-get upgrade -y; apt-get dist-upgrade -y; apt-get autoremove -y; apt-get autoclean -y

# Service(s)
ADD supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# Setup
RUN apt-get -y install squid privoxy tor polipo || true
ADD privoxy.conf /etc/privoxy/config
ADD tor.conf /etc/tor/torrc
ADD squid.conf /etc/squid3/squid.conf
ADD polipo.conf /etc/polipo/conf

CMD ["/usr/bin/supervisord"]
